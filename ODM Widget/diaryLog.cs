using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.ODM_Widget
{
    public class diaryLog
    {
        //Slots
        public double time_count = 0f;
        public int dead_count = 0;

        //Real data body
        public List<string> resist = new List<string>();
        public List<string> feed = new List<string>();
        public List<string> resist_failed = new List<string>();
        public List<string> resist_success = new List<string>();
        public List<string> feed_failed = new List<string>();
        public List<string> feed_success = new List<string>();
        public List<string> killer_name = new List<string>();

        public diaryLog()
        {

        }
        public diaryLog(string _json)
        {
            if (!String.IsNullOrEmpty(_json))
            {
                //Create an instance
                diaryLog db = JsonMapper.ToObject<diaryLog>(_json);
                //Map data from the instance
                this.time_count = db.time_count;
                this.dead_count = db.dead_count;
                this.resist = db.resist;
                this.feed = db.feed;
                this.resist_success = db.resist_success;
                this.feed_failed = db.feed_failed;
                this.feed_success = db.feed_success;
                this.killer_name = db.killer_name;
            }
        }
        public string getJsonString()
        {
            if (this != null)
            {
                JsonData jsonData = JsonMapper.ToJson(this);
                return jsonData.ToString();
            }
            return "null";
        }

        #region LINQ
        public string get_best_death()
        {
            string killer = "-";
            if (killer_name.Count > 0)
            {
                killer = (from i in killer_name
                          group i by i into grp
                          orderby grp.Count() descending
                          select grp.Key).First();
                killer = dataWidget.getTranslaton(killer);
            }
            return killer;
        }
        public int get_best_death_count()
        {
            int best_death_count = 0;
            if (killer_name.Count > 0)
            {
                best_death_count = (from i in killer_name
                                    group i by i into grp
                                    orderby grp.Count() descending
                                    select grp.Key).Count();
            }
            return best_death_count;
        }
        public string get_mate_count()
        {
            return resist.Count.ToString();
        }
        public string get_best_mate()
        {
            string mate = "-";
            if (resist.Count > 0)
            {
                mate = (from i in resist
                        group i by i into grp
                        orderby grp.Count() descending
                        select grp.Key).First();
                mate = dataWidget.getTranslaton(mate);
            }
            return mate;
        }
        public int get_best_mate_count()
        {
            int mate_count = 0;
            if (resist.Count > 0)
            {
                mate_count = (from i in resist
                              group i by i into grp
                              orderby grp.Count() descending
                              select grp.Key).Count();
            }
            return mate_count;
        }
        public int get_climax_count()
        {
            return resist_failed.Count();
        }
        public string get_best_climax()
        {
            string get_climax_mate = "-";
            if (resist_failed.Count > 0)
            {
                get_climax_mate = (from i in resist_failed
                                   group i by i into grp
                                   orderby grp.Count() descending
                                   select grp.Key).FirstOrDefault();
                get_climax_mate = dataWidget.getTranslaton(get_climax_mate);
            }
            return get_climax_mate;
        }
        public int get_best_climax_count()
        {
            int climax_count = 0;
            if (resist_failed.Count > 0)
            {
                climax_count = (from i in resist_failed
                                group i by i into grp
                                orderby grp.Count() descending
                                select grp.Key).Count();
            }
            return climax_count;
        }
        public int get_feed_count()
        {
            return feed.Count;
        }
        public string get_best_feed()
        {
            string best_feed_mate = "-";
            if (feed.Count > 0)
            {
                best_feed_mate = (from i in feed
                                  group i by i into grp
                                  orderby grp.Count() descending
                                  select grp.Key).First();
                best_feed_mate = dataWidget.getTranslaton(best_feed_mate);
            }
            return best_feed_mate;
        }
        public string get_best_feed_success()
        {
            string best_feed_success_mate = "-";
            if (feed_success.Count > 0)
            {
                best_feed_success_mate = (from i in feed_success
                                          group i by i into grp
                                          orderby grp.Count() descending
                                          select grp.Key).First();
                best_feed_success_mate = dataWidget.getTranslaton(best_feed_success_mate);
            }
            return best_feed_success_mate;
        }
        public string get_best_feed_failure()
        {
            string best_feed_failure_mate = "-";
            if (feed_failed.Count > 0)
            {
                best_feed_failure_mate = (from i in feed_failed
                                          group i by i into grp
                                          orderby grp.Count() descending
                                          select grp.Key).First();
                best_feed_failure_mate = dataWidget.getTranslaton(best_feed_failure_mate);
            }
            return best_feed_failure_mate;
        }
        #endregion
    }

}
