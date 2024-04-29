SET @row_number = 0;  
SET @sortField = '[FieldId]';

select StoreId from (
	select StoreId, min(indexSort) as indexSort from (
	select store.*, StoreId, indexSort from (
	select StoreId, FieldId, Result , IF(@sortField = FieldId, (@row_number := @row_number + 1), 9999999999999999999999) AS indexSort from (
	select StoreId, FieldId, Result from mtd_store_text 
	where IsDeleted = 0 
	union all
	select StoreId, FieldId, group_concat(Result SEPARATOR '') as Result from mtd_store_memo 
	where IsDeleted = 0 
	group by StoreId, FieldId
	union all
	select StoreId, FieldId, Result from mtd_store_int 
	where IsDeleted = 0 
	union all
	select StoreId, FieldId, Result from mtd_store_date 
	where IsDeleted = 0 
	union all
	select StoreId, FieldId, Result from mtd_store_decimal 
	where IsDeleted = 0 
	union all
	select StoreId, FieldId, FileName as Result from mtd_store_file 
	where IsDeleted = 0
	union all
    select StoreId, FieldId, ItemId as Result from mtd_store_item 
	where IsDeleted = 0
	) as f
    /*inner join mtd_store_owner*/
	where Result like '%%' /*and FieldId in*/ /*and Result*/ /*and StoreId*/
	order by Result, FieldId
) as d 
inner join mtd_store as store on store.id = d.StoreId and store.IsDeleted = 0
/*left join mtd_store_approval*/
where store.mtd_form = '[FormId]' /*and store.timecr between*/
/*order by store.timecr desc*/
/*order by sequence desc*/
/*order by approval.md_approve_stage desc*/
) as sorting
group by StoreId
/*order by indexSort*/
limit 0,10
) as final