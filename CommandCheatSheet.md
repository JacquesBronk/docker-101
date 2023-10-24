### Basic Commands
- `docker --version`: Display Docker version information.
- `docker info`: Display detailed information on the Docker installation.

### Images
- `docker images`: List all local images.
- `docker pull [IMAGE_NAME]`: Pull an image from a registry.
- `docker rmi [IMAGE_NAME]`: Remove a local image.
- `docker build -t [IMAGE_NAME] .`: Build an image from a Dockerfile in the current directory.

### Containers
- `docker ps`: List running containers.
- `docker ps -a`: List all containers (both running and stopped).
- `docker run [IMAGE_NAME]`: Run a command in a new container.
- `docker start [CONTAINER_ID/NAME]`: Start a stopped container.
- `docker stop [CONTAINER_ID/NAME]`: Stop a running container.
- `docker restart [CONTAINER_ID/NAME]`: Restart a container.
- `docker rm [CONTAINER_ID/NAME]`: Remove a container.

### Executing Commands
- `docker exec -it [CONTAINER_ID/NAME] [COMMAND]`: Execute a command inside a running container.

### Logs
- `docker logs [CONTAINER_ID/NAME]`: Fetch the logs of a container.

### Network
- `docker network ls`: List networks.
- `docker network create [NETWORK_NAME]`: Create a new network.
- `docker network connect [NETWORK_NAME] [CONTAINER_ID/NAME]`: Connect a container to a network.
- `docker network disconnect [NETWORK_NAME] [CONTAINER_ID/NAME]`: Disconnect a container from a network.

### Volumes
- `docker volume ls`: List volumes.
- `docker volume create [VOLUME_NAME]`: Create a new volume.
- `docker volume inspect [VOLUME_NAME]`: Display detailed information on a specific volume.

### System
- `docker system df`: Show Docker disk usage.
- `docker system prune`: Remove all unused containers, networks, images (both dangling and unreferenced), and optionally, volumes.

### Docker-Compose
- `docker-compose up`: Create and start containers as defined in the `docker-compose.yml`.
- `docker-compose down`: Stop and remove resources created by `up`.
- `docker-compose build`: Build or rebuild services.

### Miscellaneous
- `docker cp [CONTAINER_ID/NAME]:[CONTAINER_PATH] [HOST_PATH]`: Copy files/folders from a container's filesystem to the host path.
- `docker cp [HOST_PATH] [CONTAINER_ID/NAME]:[CONTAINER_PATH]`: Copy files/folders from the host's filesystem to the container path.

**Note:** This cheat sheet provides a basic overview of common Docker commands. 
The actual list is extensive, and the usage might vary based on the specific configurations and what you are trying to achieve. 
Always refer to the official Docker documentation for a comprehensive list of commands and their correct usage.
