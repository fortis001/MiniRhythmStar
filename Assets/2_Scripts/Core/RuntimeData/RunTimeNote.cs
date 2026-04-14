

namespace MyGame.Core.RuntimeData
{
    public class RuntimeNote
    {
        public float Time { get; }
        public int Lane { get; set; }
        public int Type { get; }

        public RuntimeNote(float time, int lane, int type)
        {
            Time = time;
            Lane = lane;
            Type = type;
        }
    }
}
