using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.IntefaceFramework.WebApi.assembly
{
	public class Test1
	{

		public string SaveData(Test1MainModel MainModel)
		{
		 return "成功！";
	
		}



	}


	
public class Test1MainModel
{
			public string  billID {get;set;}
			public string  Code {get;set;}
			public string  Name {get;set;}
			public List<Test1DataSubModel>  Data {get;set;}
			
}













	public class Test1DataSubModel
	{
			public string  DTLCODE {get;set;}
			public string  DTLNAME {get;set;}
		}







 



}
