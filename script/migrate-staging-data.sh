#!/usr/bin/env bash
# DESCRIPTION: Helper script to run debugging environment

export ASPNETCORE_ENVIRONMENT=Staging

set -e

echo "Migrate Data DB."
cd ".\..\Service.Data"

until dotnet ef database update; do
>&2 echo "DB Data is migrated"
sleep 1
done

read junk