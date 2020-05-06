

# **Examen Mercadolibre**

Archivo con requerimientos: *Challenge MeLi BE- Mutantes.pdf*

Fecha: 29/Abril/2020

#### Descripción de la Solución 

La solución es Cloud Native basada en Azure Functions (https://azure.microsoft.com/en-us/services/functions/) y Azure CosmosDB (https://azure.microsoft.com/en-us/services/cosmos-db/). Ambos servicios permiten implementar una arquitectura Event-Driven.

Para el Web API (/mutant, /stats), se utilizó un runtime Python sobre Linux que es alocado cuando recibe tráfico, y es elástico escalando horizontalmente a demanda. En caso de que la API no sea utilizada durante unos minutos pasa a un estado Cold (el warmup demora unos milisegundos).

Para la base de datos se utilizo una API SQL con dos Collections donde se almacenan los ADN mutantes y humanos como documentos.

**Directorios**

* *function-app*: Azure Serverless API con la solución a los desafíos Nivel 2 y 2
* *azure_api_tests*: Azure API Tests para testear con *newman* 
* *mutant-algorithm-python*: Notebook con el algoritmo en Python con UnitTests para el desafío Nivel 1

**URLs REST API**

  * URL Base: https://mutant-meli.azurewebsites.net/ 
  * URL Mutant Endpoint: https://mutant-meli.azurewebsites.net/api/mutant [POST] 
  * URL Stats Endpoint: https://mutant-meli.azurewebsites.net/api/stats [GET] 

Nota: no se retorna error cuando se utiliza el método equivocado en los endpoint.



**ESCALABILIDAD**

Para atender el requerimiento de escalabilidad de 1M req/sec, la API tiene dos componentes Cloud Native de Microsoft escalables independientes: Web API y Base de Datos.

1) La *Web API* es un runtime Linux/Python que escala según se requiera. Para este caso la configuración utilizada es por default. Pero de ser necesario se puede hacer fine-tunning con los siguientes detalles:

​         https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-python

​		https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale

​		https://docs.microsoft.com/en-us/azure/azure-functions/functions-best-practices#scalability-best-practices

2) La *Base de Datos* es una base de datos documental con API SQL y particionada para mutantes y humanos. Para el row_id dentro de una partición se usan 60 bytes. Esto significa que puede almacenar aproximadamente 10^144 ADNs mutantes + 10^144 ADN humanos.



**GITHUB**

https://github.com/olivetom/meli-mutant-challenge

6/Abril/2020

* Added C# Stateful Orchestrator Workflow as an alternative API endpoint located at https://meli-stats-orchestrator.azurewebsites.net/api/stats