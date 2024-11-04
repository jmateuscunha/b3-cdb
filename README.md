
# CDB Earning Calculator

## How to Run Using Docker Compose

 docker-compose up -d

#### CDB

```http
  POST /api/cdb/calculate
```
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

