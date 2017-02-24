using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Client.Models.Utils.DAL.Common
{
    public class NavigationHelper<T>
        where T : class, IDerivedEntity
    {
        public NavigationHelper()
        {
        }

        private List<string> paths = new List<string>();

        public static NavigationHelper<T> Get()
        {
            return new NavigationHelper<T>();
        }

        public NavigationHelper<T> Include(Expression<Func<T, object>> path)
        {
            this.GetPath(path.ToString());
            return this;
        }

        public NavigationHelper<T> Include(Expression<Func<T, List<object>>> path)
        {
            this.GetPath(path.ToString());
            return this;
        }

        public string[] All()
        {
            var result = this.paths;
            this.paths = new List<string>();
            return result.ToArray();
        }

        public string Single()
        {
            var result = string.Empty;
            if (this.paths.Count > 0)
            {
                result = this.paths[0];
                this.paths = new List<string>();
            }
            return result;
        }

        private void GetPath(string expression)
        {
            var result = this.TrimBody(expression);
            this.paths.Add(result);
        }

        private string TrimBody(string body)
        {
            var result = body.Split(new string[] { "=>" }, StringSplitOptions.None)[1].Replace(".Select()", string.Empty);
            var paramIndex = result.IndexOf(".");
            return result.Substring(paramIndex + 1).Trim();
        }
    }

    //class Test
    //{
    //    void Method()
    //    {
    //        var lot = NavigationHelper<MatTblLoturi>.get();
    //        lot.include((it) => it.MatTblMaterialeArticole.MatTblDepartFunctAsocs.Select().OrgTblDepartamente);
    //        lot.include((it) => it.MatTblMaterialeArticole);
    //    }
    //}

}
