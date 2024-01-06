# pull and run docker registry in local docker
docker run -d -p 5000:5000 --restart=always --name registry registry:latest

# To see log
docker logs -f registry 


// asp.net app
  // Configure the HTTP request pipeline.
  //if (app.Environment.IsDevelopment())
  //{
      app.UseSwagger();
      app.UseSwaggerUI();
  //}

// build images

docker build -t private-registry-demo:1.0.0 .

// Run image in a container 

docker run -dp 8080:8080 --name netcore-grapprivate-registry-demo private-registry-demo:1.0.0

// Browse application

http://localhost:8080/swagger/index.html

// Or

http://localhost:8080/WeatherForecast


docker tag privateregistrydemo:1.0.0 localhost:5000/privateregistrydemo
docker push localhost:5000/privateregistrydemo
docker pull localhost:5000/privateregistrydemo
docker run -dp 8080:8080 --name private-registry-demo localhost:5000/privateregistrydemo:latest

// Browse application

http://localhost:8080/swagger/index.html

// Or

http://localhost:8080/WeatherForecast


References:
Your own private Docker registry with Portus

https://github.com/SUSE/Portus

https://www.youtube.com/watch?v=gvaRfAqCfGY

https://www.howtogeek.com/googles-password-manager-lands-in-the-pixel-launcher/
https://www.codeproject.com/Articles/1263817/How-to-Setup-Our-Own-Private-Docker-Registry

//Docker Registry Tutorial - Docker registry - Setup WebUI to a private docker registry server
https://www.youtube.com/watch?v=rbdQeGYlwyY

// Host your own docker registry | Local Docker Registry | Docker Registry using Docker Compose
https://www.youtube.com/watch?v=8gEs_zefNYA
https://www.youtube.com/watch?v=HqzQ3p4QXwQ  // using compose file

https://pkuwwt.github.io/techniques/2020-04-04-setup-a-private-docker-registry/

https://www.youtube.com/watch?v=bSVm69njiTg
