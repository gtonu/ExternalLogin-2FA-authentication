namespace DevSkill.Blog.Domain.Utilities.DataTable
{
    public interface IDataTables
    {
        static object EmptyResult { get; }
        int Length { get; set; }
        SortColumn[] Order { get; set; }
        int PageIndex { get; }
        int PageSize { get; }
        DataTablesSearch Search { get; set; }
        int Start { get; set; }

        string? FormatSortExpression(params string[] columns);
    }
}