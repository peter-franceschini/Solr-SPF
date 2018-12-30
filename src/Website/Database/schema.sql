-- Entity Framework Core update database first models
-- Scaffold-DbContext "Server=.\SQLEXPRESS;Database=SolrQueryExplain;user id=solr;password=solr;MultipleActiveResultSets=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/Database -Force

create table SolrQueryResponseRecord
(
	Id int Identity(1,1) primary key not null,
	UserId nvarchar(450) foreign key references AspNetUsers(Id),
	GUID nvarchar(50),
	CreateDateTime DateTime,
	SolrQuery nvarchar(max),
	SolrQueryResponse nvarchar(max)
)