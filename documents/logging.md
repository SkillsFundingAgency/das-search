# Logging

## Indexer

Properties: 
- Application: "Sfa.Das.Indexer"
- assembly-version: 

- Level: log level (debug, info, warn, error, fatal)
- Identifier: 
  - DocumentCount -> Info message is sent when all docuemtns are sent to the new index, Using properties *DocumentType* and *TotalCount*. 

- Message: 
  - ElasticsearchQuery: *identifier* -> debug, logging calls to elasticsearch with *HttpStatusCode*, *ResponseTime*, *Uri* and *RequestBody* if available.

- DocumentType:
- TotalCount: **int**

## Web

Properties: 
- Application: "Sfa.Das.Web"

- Level: log level (debug, info, warn, error, fatal)
- Identifier: 
  - Elasticsearch.Search.*caller* -> logging calls to elasticsearch with *HttpStatusCode*, *ResponseTime*, *Uri* and *RequestBody* if available.
  - Elasticsearch.Search.SearchByKeyword
  - Elasticsearch.Search.GetFrameworkById
  - Elasticsearch.Search.GetStandardById
  - Elasticsearch.Search.GetProvider
  - Elasticsearch.Search.SearchByFrameworkLocation


