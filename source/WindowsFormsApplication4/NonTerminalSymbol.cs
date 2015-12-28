using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
    
   public  class NonTerminalSymbol
    {

        private string value;
        private HashSet<String> first;
        public  HashSet<String> followList;
        private HashSet<String> synList;
        public  HashSet<NonTerminalSymbol> higherNTS;
        private bool generateNull;
        private HashSet<Table>table=new  HashSet<Table>();
        private HashSet<string> tablequene = new HashSet<string>();

        public NonTerminalSymbol(String value)
        {
            this.value = value;

            this.first = new HashSet<string>();
            this.followList = new HashSet<string>();
            this.synList = new HashSet<string>();
            this.higherNTS = new HashSet<NonTerminalSymbol>();
            generateNull = false;
        }

        public HashSet<NonTerminalSymbol> getHigherNTS()
        {
            return higherNTS;
        }
        public void setGenerateNull(bool flag)
        {
            this.generateNull=true;;

        }
        public bool getGenerateNull()
        {
           return  generateNull  ;

        }

        public void setHigherNTS(HashSet<NonTerminalSymbol> higherNTS)
        {
            this.higherNTS = higherNTS;
        }

        public HashSet<string> getSynList()
        {
            return synList;
        }

        public void setHashSet(HashSet<string> synList)
        {
            this.synList = synList;
        }

        public string getValue()
        {
            return value;
        }

        public void setValue(string value)
        {
            this.value = value;
        }

        public HashSet<string> getFirst()
        {
            return first;
        }

        public void setFirst(HashSet<string> first)
        {
            this.first = first;
        }
        public HashSet<string> getTableQuene()
        {
            tablequene.UnionWith(first);
            tablequene.UnionWith(followList);
            tablequene.Add("@");
            return tablequene;
            
            //return value;
        }

        public HashSet<string> getFollowList()
        {
            return followList;
        }
        public void  unionFollowList(HashSet<string> otherlist)
        {
            followList.UnionWith(otherlist);
            
        }
        public void unionFirstList(HashSet<string> otherlist)
        {
            first.UnionWith(otherlist);

        }

        public void setFollowList(HashSet<string> followList)
        {
            this.followList = followList;
        }

          public void setTable(HashSet<Table> t)
          {
              this.table=t;
          }
          public HashSet<Table> getTable()
          {
              return table;
          }
        public bool Equals(NonTerminalSymbol other)
        {


            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return (other.value == value);

         }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(NonTerminalSymbol))
            {
                return false;
            }

            return Equals((NonTerminalSymbol)obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(NonTerminalSymbol left, NonTerminalSymbol right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NonTerminalSymbol left, NonTerminalSymbol right)
        {
            return !Equals(left, right);
        }
    



        

       
       
    
    }

}
