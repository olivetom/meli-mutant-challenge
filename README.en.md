###### Spanish version: [README.md] (https://github.com/olivetom/meli-mutant-challenge/README.md)

# ** Mercadolibre Exam **

File with requirements: * Challenge MeLi BE- Mutants.pdf *

Date: April 29, 2020

#### Solution Description

The solution is Cloud Native based on Azure Functions (https://azure.microsoft.com/en-us/services/functions/) and Azure CosmosDB (https://azure.microsoft.com/en-us/services/cosmos -db /). Both services allow to implement an Event-Driven architecture.

For the Web API (/ mutant, / stats), we used a Python runtime on Linux that is crazy when it receives traffic, and is elastic scaling horizontally on demand. If the API is not used for a few minutes, it goes into a Cold state (the warmup takes a few milliseconds).

For the database we used an SQL API with two Collections where mutant and human DNA are stored as documents.

** Directories **

* function-app *: Azure Serverless API with solution to Level 2 and 2 challenges
* azure_api_tests *: Azure API Tests to test with * newman *
* mutant-algorithm-python *: Notebook with the algorithm in Python with UnitTests for the Level 1 challenge

** REST API URLs **

  * Base URL: https://mutant-meli.azurewebsites.net/
  * Mutant Endpoint URL: https://mutant-meli.azurewebsites.net/api/mutant [POST]
  * URL Stats Endpoint: https://mutant-meli.azurewebsites.net/api/stats [GET]

Note: no error is returned when using the wrong method on endpoints.



** SCALABILITY **

To meet the scalability requirement of 1M req / sec, the API has two independent scalable Microsoft Cloud Native components: Web API and Database.

1) The * Web API * is a Linux / Python runtime that scales as required. In this case, the configuration used is by default. But if necessary, fine-tuning can be done with the following details:

Https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-python

Https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale

Https://docs.microsoft.com/en-us/azure/azure-functions/functions-best-practices#scalability-best-practices

2) The * Database * is a documentary database with SQL API and partitioned for mutants and humans. For the row_id within a partition 60 bytes are used. This means that it can store approximately 10 ^ 144 mutant DNAs + 10 ^ 144 human DNA.


** GITHUB **

https://github.com/olivetom/meli-mutant-challenge

6 / April / 2020

* Added C # Stateful Orchestrator Workflow as an alternative API endpoint located at https://meli-stats-orchestrator.azurewebsites.net/api/stats

May 19, 2020

* Added support of request body json dna string array. Dna should be in the form: {"dna": ["ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG"]}. Tests modified accordingly.