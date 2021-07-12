using System;
namespace DC.Core.Contracts.Application.Pipeline.Dtos.Input
{
	public class SearchPaginationRequestDto<T> where T : class
	{
		public T RestrictionCriteria { get; set; }
		public string OrderBy { get; set; }
		public bool OrderDescending { get; set; } = true;
		public int PageNumber { get; set; }
		public int RowsPerPage { get; set; }
	}
}
