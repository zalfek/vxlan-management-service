#!/bin/bash


# Read Username
echo -n Username: 
read -s username
echo
# Read Password
echo -n Password: 
read -s password
echo

# Request a token
json=$(curl --location --request POST 'https://login.microsoftonline.com/6b2cc106-2159-459a-9474-f37f9153962d/oauth2/v2.0/token' \
--header 'Content-Type: application/x-www-form-urlencoded' \
-d "grant_type=password&password=$password&username=$username&client_id=0ee06e10-7b2e-41ac-8cda-467b0fa54d7f&scope=api://e9663b27-fbcb-4826-9db0-4efb9f8f7c1a/access_as_user" )

echo $json