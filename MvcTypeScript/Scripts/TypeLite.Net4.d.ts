
 
 

 

/// <reference path="Enums.ts" />

declare module MvcTypeScript.Models.Todo {
	interface ITodoListenViewModel {
		PriorityList: MvcTypeScript.Helper.IListItemIntEntry[];
	}
	interface ITodoOverviewInitModel {
		Datum: string;
		CurrentUser: string;
		EmptyTodoEntry: MvcTypeScript.Models.Todo.ITodoCreateViewModel;
	}
	interface ITodoCreateViewModel {
		Id: number;
		IsActive: boolean;
		Description: string;
		DoDate: Date;
		Creator: string;
		Prioritaet: MvcTypeScript.Helper.Prioritaet;
	}
	interface ITodoOverviewResultModel {
		Entries: MvcTypeScript.Models.Todo.Container.ITodoEntry[];
	}
	interface ITodoOverviewSearchModel {
		SearchText: string;
		PageSize: number;
		Startdatum: Date;
		Enddatum: Date;
		IsActive: boolean;
	}
}
declare module MvcTypeScript.Helper {
	interface IListItemIntEntry {
		Selected: boolean;
		Text: string;
		Value: number;
	}
}
declare module MvcTypeScript.Models.Todo.Container {
	interface ITodoEntry {
		Id: number;
		IsOpen: boolean;
		Description: string;
		DoDate: string;
		Creator: string;
		Prioritaet: MvcTypeScript.Helper.Prioritaet;
	}
}


