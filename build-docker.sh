#!/usr/bin/env bash


echo "Build Data Service"
docker build -f Service.Data/Dockerfile -t rivnd/vip-data-service:dev .

echo "Build Gateway CMS"
docker build -f Gateway.CMS/Dockerfile -t rivnd/vip-cms-gateway:dev .


