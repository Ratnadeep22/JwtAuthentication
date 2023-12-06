# JwtAuthentication

## clone repository with below command:
```
git clone https://github.com/Ratnadeep22/JwtAuthentication.git
```

## Run solution:

## To generate token: Open postman and call below API call:
```
https://localhost:44307/api/TokenGenerator/GenerateToken

Body:
{
    "UserName":"RSK",
    "Password": "RSK"
}
```

## Copy the token from response message and run call below API by selecting the authorization type as "Bearer Token":
```
https://localhost:44307/api/TokenGenerator/TestToken
```
## Once you execute it, You will get an response message as:
```
User is successfully authenticated...
```
