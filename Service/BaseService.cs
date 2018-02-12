using System;
using Model;
using SqlSugar;

namespace Service
{
    public class BaseService
    {
        public SqlSugarClient DB { get; set; }

        public BaseService()
        {
            DB = SugarBase.DB;
        }
    }
}
