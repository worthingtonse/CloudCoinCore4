using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Foundation
{
    class RAIDA
    {
        /* INSTANCE VARIABLE */
        public DetectionAgent[] agent;
        public CloudCoin returnCoin;
        public Response[] responseArray = new Response[25];
        private int[] working_triad = { 0, 1, 2 };//place holder
        public bool[] raidaIsDetecting = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
        public string[] lastDetectStatus = { "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected" };
        public string[] echoStatus = { "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply" };
       
 
        
        /* CONSTRUCTOR */
        public RAIDA(int milliSecondsToTimeOut)
        { //  initialise instance variables
            this.agent = new DetectionAgent[25];
            for (int i = 0; (i < 25); i++)
            {
                this.agent[i] = new DetectionAgent(i, milliSecondsToTimeOut);
            } // end for each Raida
        }//End Constructor


        public void echoOne( int raida_id )
        {
            DetectionAgent da = new DetectionAgent(raida_id, 2000);
            responseArray[raida_id] = da.echo(raida_id);
        }//end echo 




        public Response[] echoAll(int milliSecondsToTimeOut )
           {
       

            Response[] results = new Response[25];
            var t00 = Task.Factory.StartNew(() => echoOne(00));
            var t01 = Task.Factory.StartNew(() => echoOne(01));
            var t02 = Task.Factory.StartNew(() => echoOne(02));
            var t03 = Task.Factory.StartNew(() => echoOne(03));
            var t04 = Task.Factory.StartNew(() => echoOne(04));
            var t05 = Task.Factory.StartNew(() => echoOne(05));
            var t06 = Task.Factory.StartNew(() => echoOne(06));
            var t07 = Task.Factory.StartNew(() => echoOne(07));
            var t08 = Task.Factory.StartNew(() => echoOne(08));
            var t09 = Task.Factory.StartNew(() => echoOne(09));
            var t10 = Task.Factory.StartNew(() => echoOne(10));
            var t11 = Task.Factory.StartNew(() => echoOne(11));
            var t12 = Task.Factory.StartNew(() => echoOne(12));
            var t13 = Task.Factory.StartNew(() => echoOne(13));
            var t14 = Task.Factory.StartNew(() => echoOne(14));
            var t15 = Task.Factory.StartNew(() => echoOne(15));
            var t16 = Task.Factory.StartNew(() => echoOne(16));
            var t17 = Task.Factory.StartNew(() => echoOne(17));
            var t18 = Task.Factory.StartNew(() => echoOne(18));
            var t19 = Task.Factory.StartNew(() => echoOne(19));
            var t20 = Task.Factory.StartNew(() => echoOne(20));
            var t21 = Task.Factory.StartNew(() => echoOne(21));
            var t22 = Task.Factory.StartNew(() => echoOne(22));
            var t23 = Task.Factory.StartNew(() => echoOne(23));
            var t24 = Task.Factory.StartNew(() => echoOne(24));

            var taskList = new List<Task> { t00, t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24 };
            Task.WaitAll(taskList.ToArray(), milliSecondsToTimeOut);
            return results;
        }//end echo



        public void detectOne(int raida_id, int nn, int sn, String an, String pan, int d)
        {
            DetectionAgent da = new DetectionAgent(raida_id, 5000);
            responseArray[raida_id] = da.detect(nn, sn, an, pan, d);
        }//end detectOne



        public CloudCoin detectCoin(CloudCoin cc, int milliSecondsToTimeOut)
        {
            returnCoin = cc;

            var t00 = Task.Factory.StartNew(() => detectOne(00, cc.nn, cc.sn, cc.ans[00], cc.pans[00], cc.getDenomination()));
            var t01 = Task.Factory.StartNew(() => detectOne(01, cc.nn, cc.sn, cc.ans[01], cc.pans[01], cc.getDenomination()));
            var t02 = Task.Factory.StartNew(() => detectOne(02, cc.nn, cc.sn, cc.ans[02], cc.pans[02], cc.getDenomination()));
            var t03 = Task.Factory.StartNew(() => detectOne(03, cc.nn, cc.sn, cc.ans[03], cc.pans[03], cc.getDenomination()));
            var t04 = Task.Factory.StartNew(() => detectOne(04, cc.nn, cc.sn, cc.ans[04], cc.pans[04], cc.getDenomination()));
            var t05 = Task.Factory.StartNew(() => detectOne(05, cc.nn, cc.sn, cc.ans[05], cc.pans[05], cc.getDenomination()));
            var t06 = Task.Factory.StartNew(() => detectOne(06, cc.nn, cc.sn, cc.ans[06], cc.pans[06], cc.getDenomination()));
            var t07 = Task.Factory.StartNew(() => detectOne(07, cc.nn, cc.sn, cc.ans[07], cc.pans[07], cc.getDenomination()));
            var t08 = Task.Factory.StartNew(() => detectOne(08, cc.nn, cc.sn, cc.ans[08], cc.pans[08], cc.getDenomination()));
            var t09 = Task.Factory.StartNew(() => detectOne(09, cc.nn, cc.sn, cc.ans[09], cc.pans[09], cc.getDenomination()));
            var t10 = Task.Factory.StartNew(() => detectOne(10, cc.nn, cc.sn, cc.ans[10], cc.pans[10], cc.getDenomination()));
            var t11 = Task.Factory.StartNew(() => detectOne(11, cc.nn, cc.sn, cc.ans[11], cc.pans[11], cc.getDenomination()));
            var t12 = Task.Factory.StartNew(() => detectOne(12, cc.nn, cc.sn, cc.ans[12], cc.pans[12], cc.getDenomination()));
            var t13 = Task.Factory.StartNew(() => detectOne(13, cc.nn, cc.sn, cc.ans[13], cc.pans[13], cc.getDenomination()));
            var t14 = Task.Factory.StartNew(() => detectOne(14, cc.nn, cc.sn, cc.ans[14], cc.pans[14], cc.getDenomination()));
            var t15 = Task.Factory.StartNew(() => detectOne(15, cc.nn, cc.sn, cc.ans[15], cc.pans[15], cc.getDenomination()));
            var t16 = Task.Factory.StartNew(() => detectOne(16, cc.nn, cc.sn, cc.ans[16], cc.pans[16], cc.getDenomination()));
            var t17 = Task.Factory.StartNew(() => detectOne(17, cc.nn, cc.sn, cc.ans[17], cc.pans[17], cc.getDenomination()));
            var t18 = Task.Factory.StartNew(() => detectOne(18, cc.nn, cc.sn, cc.ans[18], cc.pans[18], cc.getDenomination()));
            var t19 = Task.Factory.StartNew(() => detectOne(19, cc.nn, cc.sn, cc.ans[19], cc.pans[19], cc.getDenomination()));
            var t20 = Task.Factory.StartNew(() => detectOne(20, cc.nn, cc.sn, cc.ans[20], cc.pans[20], cc.getDenomination()));
            var t21 = Task.Factory.StartNew(() => detectOne(21, cc.nn, cc.sn, cc.ans[21], cc.pans[21], cc.getDenomination()));
            var t22 = Task.Factory.StartNew(() => detectOne(22, cc.nn, cc.sn, cc.ans[22], cc.pans[22], cc.getDenomination()));
            var t23 = Task.Factory.StartNew(() => detectOne(23, cc.nn, cc.sn, cc.ans[23], cc.pans[23], cc.getDenomination()));
            var t24 = Task.Factory.StartNew(() => detectOne(24, cc.nn, cc.sn, cc.ans[24], cc.pans[24], cc.getDenomination()));


            var taskList = new List<Task> { t00, t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11 , t12 , t13 , t14 , t15 , t16 , t17 , t18 , t19 , t20 , t21 , t22 , t23,  t24   };
            Task.WaitAll( taskList.ToArray(), milliSecondsToTimeOut);
            //Get data from the detection agents

            for (int i = 0; i < 25; i++)
            {
                if ( responseArray[i] !=null ) {
                    returnCoin.setPastStatus(responseArray[i].outcome, i);
                } else {
                    returnCoin.setPastStatus("undetected", i);
                };// should be pass, fail, error or undetected. 
            }//end for each detection agent

            returnCoin.setAnsToPansIfPassed();
            returnCoin.calculateHP();
            returnCoin.gradeCoin(); // sets the grade and figures out what the file extension should be (bank, fracked, counterfeit, lost
            returnCoin.calcExpirationDate();
            returnCoin.grade();

            return returnCoin;
        }//end detect coin


        public void get_Ticket(int i, int raidaID, int nn, int sn, String an, int d) {
            DetectionAgent da = new DetectionAgent(raidaID, 5000);
            responseArray[raidaID] = da.get_ticket(nn, sn, an, d);
        }//end get ticket


        public void get_Tickets(int[] triad, String[] ans, int nn, int sn, int denomination, int milliSecondsToTimeOut)
        {
            //Console.WriteLine("Get Tickets called. ");
            var t00 = Task.Factory.StartNew(() => get_Ticket(0, triad[00], nn, sn, ans[00],  denomination));
            var t01 = Task.Factory.StartNew(() => get_Ticket(1, triad[01], nn, sn, ans[01],  denomination));
            var t02 = Task.Factory.StartNew(() => get_Ticket(2, triad[02], nn, sn, ans[02],  denomination));
          
            var taskList = new List<Task> { t00, t01, t02 };
            Task.WaitAll(taskList.ToArray(), milliSecondsToTimeOut);
            //Get data from the detection agents
        }//end detect coin



    }//end RAIDA Class
}// End Namespace
