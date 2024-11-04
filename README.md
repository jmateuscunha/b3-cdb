
# CDB Earning Calculator

![image](https://github.com/user-attachments/assets/9e28166b-7875-4106-b99f-539a0769c176)

## How to Run Using Docker Compose
`
docker-compose up -d
`
#### CDB

`
POST /api/cdb/calculate
`

Request:
```json
{
  "value": 1000.66,
  "months": 24
}
```

Response:
```json
{
  "gross": 1050.60,
  "net": 1044.23
}
```

Response UnprocessableEntity:
```json
{
  "timestamp": "datetime",
  "errors": [
    "Must have at least one month set."
  ]
}
```
##

![image](https://github.com/user-attachments/assets/90612fea-accd-417c-894f-bed1c6a9b805)
