using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extension;

namespace Extension
{
    public class Cluster
    {
        Row centroid;
        List<Row> memberCluster;

        public Row Centroid
        {
            get { return centroid; }
            set { centroid = value; }
        }


        public List<Row> MemberCluster
        {
            get { return memberCluster; }
            set { memberCluster = value; }
        }

        public Cluster(Row centroid)
        {
            this.centroid = centroid;
            this.memberCluster = new List<Row>();
        }
        public Cluster()
        {
            this.centroid = null;
            this.memberCluster = new List<Row>();
        }

        public double calculateSSE()
        {
            double temp;
            double ans = 0.0;
            for (int i = 0; i < memberCluster.Count; i++)
            {
                foreach (Variables c in centroid.InputValue.Keys)
                {
                    if (memberCluster[i].InputValue.ContainsKey(c))
                    {
                        temp = Convert.ToDouble(memberCluster[i].InputValue[c].ValueCell) - Convert.ToDouble(centroid.InputValue[c].ValueCell);
                    }
                    else
                    {
                        temp = Convert.ToDouble(centroid.InputValue[c].ValueCell);
                    }
                    ans += (temp * temp);
                }
                foreach (Variables c in memberCluster[i].InputValue.Keys)
                {
                    if (!centroid.InputValue.ContainsKey(c))
                    {
                        temp = Convert.ToDouble(memberCluster[i].InputValue[c].ValueCell);
                        ans += (temp * temp);
                    }
                }                
            }
            return ans;
        }
    }

}
