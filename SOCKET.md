# 231123
- хранение коннекта ws и его метаинформации для поддержки таблицы
- загрузка произвольного батча в грид
- [x] события на вью - как отслеживать и подключиться?
- доработки DataFrame: strings
- доработки DataFrame: datetime
- команды на стороне View

# 201123
- запустить на Питоне (Ноде) вариант с большой таблицей - https://github.com/finos/perspective/tree/master/examples/python-tornado
- отдавать таблицу с сервера в формате arrow (http)

## Socket Server

- отдавать бинарный контент чанками (arrow формат)
- поддеркжа пула соединений на стороне сервиса, автологаут
- обработка пингов
- обработка текстовых команд
- хостинг таблиц (бинарные данные)
- хотсинг вьюх - пейджинг, сортировка, фильтры и проч. - через IQueryable

### Поддержка Arrow формата на стороне .Net
- read\write arrow files
- объектная модель - to arrow 
- нативные конверторы csv, json, parquet, arrow 

### Пул соединений
- подключение - добавление в список
- пинги и проверка готовности
- автологаут по таймауту

 
### DataFrame

https://zetcode.com/csharp/msa-dataframe/
https://swharden.com/blog/2022-05-01-dotnet-dataframe/


### Apache Arrow
https://github.com/apache/arrow/tree/main/csharp

### ParquetSharp
- [ParquetSharp](https://github.com/G-Research/ParquetSharp)
- [ParquetSharp.DataFrame](https://github.com/G-Research/ParquetSharp.DataFrame)


