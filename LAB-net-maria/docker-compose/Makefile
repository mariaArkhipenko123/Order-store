PATHTOSERVICES = /home/dot-net/projects/static-data/docker-compose
dev_up:
	docker-compose -f docker-compose.yml -f docker-compose.development.yml up -d --build
	docker ps

prod_up:
	docker-compose -f docker-compose.server.yml up --no-recreate -d --build
	docker-compose -f docker-compose.yml -f docker-compose.production.yml up -d --build
	docker ps

server_up:
	docker-compose -f docker-compose.server.yml up --no-recreate -d --build

server_down:
	docker-compose -f docker-compose.server.yml down
 
dev_down:
	docker-compose -f docker-compose.development.yml down  -v
	docker-compose -f docker-compose.development.yml rm -vsf

prod_down:
	docker-compose -f docker-compose.production.yml down  -v
	docker-compose -f docker-compose.production.yml rm -vsf
