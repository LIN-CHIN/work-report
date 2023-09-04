# work-report
This is a project simulating the equipment work reporting process in a factory.

## Tools :
1. `.NET Core 6 Console Application`
2. `.NET Core 6 Web API`
3. `Docker`
4. `RabbitMQ`
5. `Redis`
6. `Postgresql`
7. `Elasticsearch`
8. `Kibana`

## Projects :
1. WorkReportClient :
    - The project is for simulating actual machines or for user reporting interface.
2. WorkReportAPI :
    - This project is used to receive various work report requests.
3. RecordWorker :
    - This project is used to receive messages sent by clients and record them in the database.
4. CalculateWorker :
    - This project is used to receive work report information and calculate the total hours and machine reported pieces using Redis.
5. PublishWorker :
    - This project is used to receive computed information and send it to mobile phones via the `Line Notify API`.
6. ElasticsearchAPI :
    - This project is a simple add-on feature designed for querying logs of all work report projects so that we can quickly retrieve logs in the future.

## Line Notify Setting : 
If you want to receive work report messages on your Line app on your mobile phone, you need to first visit the Line Notify official website and link it to your Line account. Line Notify will provide you with an access token, which you should then save. Next, go to the `docker-compose.env` file, locate the `LINE_NOTIFY_ACCESS_TOKEN` field, and enter the token there.

```
LINE_NOTIFY_ACCESS_TOKEN="your access token"
```

## Deployment :

### Notic : 
- Ensure that the local ports  `15672` , `5672` , `6379` , `8081` , `5432` , `7001` , `7002` , `9200` and `5601` are not occupied by other services.
- Ensure that you have downloaded `Docker` and `Docker Compose`.

### Execute Command: 
- Click `docker-compose-up.bat` file. 

or

1. Change directory to `WorkReport` folder

```bat
cd WorkReport
``` 

2. Execute docker-compose
```bat
docker compose --env-file docker-compose.env up -d 
```

## Run : 
1. Open the `WorkReportClient`
2. Enter the machine number. `M1`, `M2` or `M3`
3. Enter the spend time(hour)
4. Enter the spend time(minute)
5. Enter the spend time(second)

Observe if each worker is functioning correctly and receive work report messages on the Line app. 

## Log Search : 
Open the url : http://localhost:7002/swagger/

