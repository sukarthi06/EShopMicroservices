export type PaginatedResult<TEntity> = { 
  pageIndex: number;
  pageSize: number;
  count: number;
  data: TEntity[]; // Use TEntity to represent the data type
};