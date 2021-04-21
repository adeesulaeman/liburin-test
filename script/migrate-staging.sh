#!/usr/bin/env bash
# DESCRIPTION: Helper script to run debugging environment

export ASPNETCORE_ENVIRONMENT=Staging

set -e

echo "Migrate Identity DB."
cd ".\..\Service.Identity"

until dotnet ef database update; do
>&2 echo "DB Identity is migrated"
sleep 1
done

read junk