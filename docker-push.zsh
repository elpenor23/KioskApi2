docker build --rm -t kioskapi2 .
docker tag kioskapi2 docker.lan:5000/kioskapi2
docker push docker.lan:5000/kioskapi2