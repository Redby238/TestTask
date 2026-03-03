Currently, the application uses Entity Framework Core with a batching mechanism (AddRangeAsync every 10,000 records) to prevent Out-Of-Memory exceptions and keep the EF Change Tracker footprint small. 
However, for a massive 10GB file, I would redesign the database insertion approach entirely

Ditch EF Core for Insertions => EF Core is an ORM and adds significant overhead due to SQL query generation and object tracking. For a 10GB file, I would replace context.SaveChanges() with ADO.NET's SqlBulkCopy.

Stream to IDataReader  => Instead of mapping CSV rows to a List<TaxiTrip> of objects, I would stream the data directly from the CsvReader into an IDataReader or a DataTable. 
SqlBulkCopy can consume an IDataReader natively, streaming the data directly into the SQL Server via the  binary protocol, which is  faster.

Memory-efficient Deduplication => The current HashSet used for deduplication stores millions of composite keys. For a 10GB file, this hashset might consume too much RAM. 
I would push the deduplication responsibility down to the database level using a temporary staging table and a SQL MERGE statement, or process the file in distributed chunks.