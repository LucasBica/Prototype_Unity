#!/bin/bash

echo "preprocess.sh execution started"

# npm auth
echo "$UPM_CONFIG_TOML" > $HOME/.upmconfig.toml

# Specify the path to the .env file
env_json_file="${WORKSPACE}/Assets/Resources/env.json"

# Creating .env file
echo "$ENV_JSON" > $env_json_file

echo "preprocess.sh execution completed"