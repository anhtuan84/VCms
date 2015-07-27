using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class SearchParameters
    {
        public const Int32 MAX_RESULT = 1000;
        public Object Query { get; private set; }
        public Int32 StartIndex { get; private set; }
        public Int32 MaxResult { get; private set; }
        public SortOrder SortOrder { get; private set; }
        public String SortField { get; private set; }

        private SearchParameters()
        {
           

        }

        public class Builder
        {
            private Object Query { get; set; }
            private Int32 StartIndex { get; set; }
            private Int32 MaxResult { get; set; }
            private SortOrder SortOrder { get; set; }
            private String SortField { get; set; }


            public Builder SetQuery(Object query)
            {
                this.Query = query;
                return this;
            }

            public Builder SetStartIndex(Int32 startIndex)
            {
                this.StartIndex = startIndex;
                return this;
            }
            public Builder SetMaxResult(Int32 maxResult)
            {
                this.MaxResult = maxResult;
                return this;
            }

            public Builder SetSortField(String sortField)
            {
                this.SortField = sortField;
                return this;
            }

            public Builder SetSortOrder(SortOrder sortOrder)
            {
                this.SortOrder = sortOrder;
                return this;
            }

            public SearchParameters Build()
            {
                Query = null;
                StartIndex = 0;
                MaxResult = MAX_RESULT;
                SortOrder = SortOrder.DESC;
                SortField = "CreatedOn";

                SearchParameters sp = new SearchParameters();
                sp.Query = this.Query;
                sp.StartIndex = this.StartIndex;
                sp.MaxResult = this.MaxResult;
                sp.SortOrder = this.SortOrder;
                sp.SortField = this.SortField;
                return sp;
            }
        }

        public static Builder getBuilder()
        {

            return new Builder();
          
        }

    }
}
