using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStrawberry3D.TK
{
    public class TKEffect : IDisposable
    {
        public static TKEffect Create(TKProgram[] programs)
        {
            return new TKEffect(programs);
        }
        public int NumPasses
        {
            get
            {
                return Programs.Count;
            }
        }
        public List<TKProgram> Programs { get; private set; }
        int _currIndex;
        public Dictionary<AttributeIdentifer, object> AttributeValues { get; set; }
        public Dictionary<UniformIdentifer, object> UniformValues { get; set; }

        TKEffect(TKProgram[] programs)
        {
            _currIndex = -1;
            Programs = new List<TKProgram>(programs);
            AttributeValues = new Dictionary<AttributeIdentifer, object>();
            UniformValues = new Dictionary<UniformIdentifer, object>();
        }
        public void SetAttributeValue(AttributeIdentifer identifer, object value)
        {
            AttributeValues[identifer] = value;
        }
        public void SetUniformValue(UniformIdentifer identifer, object value)
        {
            UniformValues[identifer] = value;
        }
        public void BeginPass(int index)
        {
            _currIndex = index;


            //检查一下有没有全数cache
            //foreach (var key in AttributeValues.Keys)
            //{
            //    if (Programs[_currIndex].AttributeIdentifers[key] == -1)
            //    {
            //        TKRenderer.Singleton.Logger.Error("Identifer " + Enum.GetName(typeof(AttributeIdentifer), key) + " not cached!");
            //    }
            //}
            //foreach (var key in UniformValues.Keys)
            //{
            //    if (Programs[_currIndex].UniformIdentifers[key] == -1)
            //    {
            //        TKRenderer.Singleton.Logger.Error("Identifer " + Enum.GetName(typeof(AttributeIdentifer), key) + " not cached!");
            //    }
            //}
            //

            Programs[index].Apply(AttributeValues, UniformValues);
        }
        public void EndPass()
        {
            Programs[_currIndex].Clear();
            _currIndex = -1;
        }
        public void Dispose()
        {
            foreach (var program in Programs)
            {
                program.Dispose();
            }
        }
    }
}
