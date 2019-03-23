properties {
    $buildVersionNumber = ""
}

task Publish -depends PublishDocker, HelmInstall

task PublishDocker {
    "Building"
    docker build ./Ncr.DeliveryKnapsack -t ncrnolo/deliveryknapsack:latest

    "Tagging"
    docker tag ncrnolo/deliveryknapsack:latest ncrnolo/deliveryknapsack:latest

    "Pushing"
    docker push ncrnolo/deliveryknapsack:latest
}

task HelmInstall {
    $fullName = docker inspect --format='{{index .RepoDigests 0}}' ncrnolo/deliveryknapsack:latest
    $tag = $fullName.Split('@')[1]
    "The tag is $tag"
    
    rm *.tgz

    "Packaging"
    helm package Build/Helm/delivery-knapsack

    "Deleting current release"
    helm delete knapsack-prod --purge

    "Publishing new release"
    gci *.tgz | % { "Publishing $_"; helm install --name knapsack-prod --set image.tag=$tag $_ }

    rm *.tgz
}