using UnityEngine;

namespace GlymorsFeasts
{
    internal struct MeatBit
    {
        public Vector2 Xz;
        public Vector3 Euler;
        public Vector3 Scale;
        public float Lift;
        public bool Juicy;
    }

    internal struct MatBit
    {
        public Vector2 Xz;
        public Vector3 Euler;
        public Vector3 Scale;
        public float Lift;
        public int Mesh;
        public bool Juicy;
    }

    internal struct StrandBit
    {
        public Vector2 Xz;
        public Vector3 Euler;
        public float Scale;
        public float Lift;
        public int Mesh;
        public bool Juicy;
    }

    internal struct ShredBit
    {
        public Vector2 Xz;
        public Vector3 Euler;
        public float Scale;
        public float Lift;
        public int Mesh;
    }

    internal struct VegBit
    {
        public int Kind; // 0 = carrot A, 1 = carrot B, 2 = turnip
        public Vector2 Xz;
        public Vector3 Euler;
        public float Scale;
    }

        internal static class Full
        {
            internal static MatBit[] Mats()
            {
                return new[]
                {
                new MatBit { Xz = new Vector2(0.0000f, 0.0000f), Euler = new Vector3(-5.6887f, 42.7227f, 3.3475f), Scale = new Vector3(1.2200f, 1.4000f, 1.1249f), Lift = 0.0078f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(0.4532f, 0.0162f), Euler = new Vector3(5.6908f, 104.0675f, -2.1709f), Scale = new Vector3(0.9301f, 0.9509f, 0.8943f), Lift = 0.0059f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(0.3781f, 0.2433f), Euler = new Vector3(3.8043f, 64.0973f, 2.5897f), Scale = new Vector3(1.1346f, 0.9529f, 1.0392f), Lift = 0.0095f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(0.1573f, 0.3425f), Euler = new Vector3(0.3936f, 54.9822f, 0.6734f), Scale = new Vector3(1.2817f, 1.0032f, 1.3788f), Lift = 0.0084f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(-0.0502f, 0.4074f), Euler = new Vector3(1.7481f, 33.8089f, 5.5544f), Scale = new Vector3(1.1022f, 0.9769f, 1.1124f), Lift = 0.0089f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2706f, 0.3679f), Euler = new Vector3(-0.4840f, 23.9584f, 6.6136f), Scale = new Vector3(0.9286f, 0.9492f, 0.9674f), Lift = 0.0104f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(-0.4056f, 0.1483f), Euler = new Vector3(5.8270f, 22.2305f, 4.0422f), Scale = new Vector3(1.0150f, 0.9629f, 1.0142f), Lift = 0.0050f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.3396f, -0.0602f), Euler = new Vector3(-10.8071f, -0.4159f, 9.2275f), Scale = new Vector3(1.1295f, 1.0333f, 1.0192f), Lift = 0.0055f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.2807f, -0.3595f), Euler = new Vector3(11.1452f, 9.9496f, -1.0005f), Scale = new Vector3(0.9574f, 0.9496f, 1.0106f), Lift = 0.0070f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0823f, -0.4156f), Euler = new Vector3(2.0328f, 29.8852f, 5.0949f), Scale = new Vector3(1.0452f, 0.9680f, 1.1033f), Lift = 0.0101f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.1534f, -0.4442f), Euler = new Vector3(-9.7458f, 67.1898f, -5.3667f), Scale = new Vector3(0.9197f, 0.9430f, 0.8575f), Lift = 0.0077f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(0.3091f, -0.2215f), Euler = new Vector3(-5.0536f, 72.0451f, -0.1088f), Scale = new Vector3(1.2328f, 1.0003f, 1.2428f), Lift = 0.0076f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(-0.1586f, -0.0246f), Euler = new Vector3(-5.0777f, 38.4995f, 8.0181f), Scale = new Vector3(1.2862f, 1.2756f, 1.2299f), Lift = 0.0124f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(0.2901f, -0.2501f), Euler = new Vector3(8.3243f, 76.4027f, -5.3126f), Scale = new Vector3(1.0271f, 0.9980f, 0.9282f), Lift = 0.0070f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.0992f, -0.0515f), Euler = new Vector3(-10.3025f, 37.4174f, 2.4166f), Scale = new Vector3(1.1617f, 1.3352f, 1.2413f), Lift = 0.0114f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(0.0415f, 0.0541f), Euler = new Vector3(-7.4469f, 41.2644f, -8.7040f), Scale = new Vector3(1.1381f, 1.3749f, 1.2166f), Lift = 0.0116f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(-0.3644f, -0.2452f), Euler = new Vector3(6.2822f, 1.5429f, -0.7874f), Scale = new Vector3(0.9535f, 0.9586f, 0.9022f), Lift = 0.0056f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(0.0506f, 0.1106f), Euler = new Vector3(2.2626f, 45.5107f, 5.6347f), Scale = new Vector3(1.0290f, 1.3242f, 0.9416f), Lift = 0.0100f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(0.0815f, 0.1788f), Euler = new Vector3(-10.5207f, 52.7575f, 7.8326f), Scale = new Vector3(1.0098f, 1.2256f, 0.9307f), Lift = 0.0121f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2348f, 0.0924f), Euler = new Vector3(-0.6780f, 5.5422f, -8.7105f), Scale = new Vector3(1.1992f, 1.1464f, 1.1002f), Lift = 0.0070f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.1462f, -0.2641f), Euler = new Vector3(-3.8847f, 21.5869f, 1.8308f), Scale = new Vector3(1.0769f, 1.0817f, 1.0188f), Lift = 0.0116f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2406f, 0.0362f), Euler = new Vector3(10.0994f, 8.4524f, 5.6375f), Scale = new Vector3(1.1893f, 1.1590f, 1.2341f), Lift = 0.0090f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.1054f, -0.0796f), Euler = new Vector3(8.5078f, 56.7428f, 6.1611f), Scale = new Vector3(1.0742f, 1.3119f, 1.1380f), Lift = 0.0066f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2983f, 0.0345f), Euler = new Vector3(-4.7048f, 12.7122f, -3.0262f), Scale = new Vector3(1.0233f, 1.0836f, 0.9307f), Lift = 0.0066f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(0.2127f, -0.2212f), Euler = new Vector3(-1.0042f, 83.4138f, -4.3853f), Scale = new Vector3(1.0478f, 1.0756f, 1.0679f), Lift = 0.0060f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.4002f, 0.2828f), Euler = new Vector3(7.9455f, 18.1930f, 5.1233f), Scale = new Vector3(1.1308f, 0.9347f, 1.0310f), Lift = 0.0075f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(0.3067f, -0.1327f), Euler = new Vector3(8.4845f, 69.4291f, -8.9318f), Scale = new Vector3(1.2836f, 1.0446f, 1.3375f), Lift = 0.0089f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0127f, -0.2022f), Euler = new Vector3(11.0045f, 50.9472f, 6.8443f), Scale = new Vector3(1.0794f, 1.2169f, 1.1528f), Lift = 0.0124f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.1960f, -0.0928f), Euler = new Vector3(8.1289f, 65.7730f, -2.4051f), Scale = new Vector3(1.0270f, 1.1965f, 0.9441f), Lift = 0.0071f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(0.2599f, 0.2281f), Euler = new Vector3(-3.1667f, 69.1911f, 9.0264f), Scale = new Vector3(1.0688f, 1.0324f, 1.0621f), Lift = 0.0071f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(0.0541f, -0.1692f), Euler = new Vector3(-9.6220f, 50.4844f, 3.2977f), Scale = new Vector3(1.0030f, 1.2522f, 1.0378f), Lift = 0.0063f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(0.1289f, 0.1584f), Euler = new Vector3(-11.5621f, 80.2345f, 8.2741f), Scale = new Vector3(1.0229f, 1.2146f, 1.0329f), Lift = 0.0116f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(0.1604f, 0.2988f), Euler = new Vector3(-5.9771f, 61.0328f, 5.6451f), Scale = new Vector3(1.2868f, 1.0393f, 1.1627f), Lift = 0.0100f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(0.2807f, 0.3639f), Euler = new Vector3(-3.9336f, 72.7757f, -6.9325f), Scale = new Vector3(1.0698f, 0.9478f, 1.0892f), Lift = 0.0096f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(-0.3562f, -0.1946f), Euler = new Vector3(0.4937f, 5.5664f, 8.1533f), Scale = new Vector3(1.0429f, 0.9801f, 0.9399f), Lift = 0.0091f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.1328f, 0.3102f), Euler = new Vector3(-8.8506f, 38.8815f, 7.2442f), Scale = new Vector3(1.2164f, 1.0411f, 1.3054f), Lift = 0.0072f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(0.1153f, 0.0059f), Euler = new Vector3(4.6298f, 51.5452f, -1.3333f), Scale = new Vector3(1.1059f, 1.3311f, 1.0114f), Lift = 0.0094f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0974f, 0.0078f), Euler = new Vector3(-5.6711f, 34.5189f, -8.7465f), Scale = new Vector3(1.1659f, 1.3497f, 1.0940f), Lift = 0.0080f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(0.0736f, -0.4844f), Euler = new Vector3(2.6448f, 56.8881f, 5.4728f), Scale = new Vector3(0.9098f, 0.9347f, 0.9541f), Lift = 0.0076f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2699f, -0.0341f), Euler = new Vector3(-0.8139f, 36.3803f, 2.4315f), Scale = new Vector3(1.2041f, 1.1197f, 1.2903f), Lift = 0.0068f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(-0.1193f, 0.1108f), Euler = new Vector3(8.9873f, 32.4179f, -3.3543f), Scale = new Vector3(1.2646f, 1.2724f, 1.1872f), Lift = 0.0123f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(0.4853f, 0.0678f), Euler = new Vector3(-8.9473f, 98.6333f, -0.6032f), Scale = new Vector3(0.9194f, 0.9347f, 0.9547f), Lift = 0.0031f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(-0.2570f, 0.3564f), Euler = new Vector3(7.9032f, 24.3673f, 8.1591f), Scale = new Vector3(0.9148f, 0.9585f, 0.9408f), Lift = 0.0091f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.4103f, -0.1061f), Euler = new Vector3(11.3489f, 26.6235f, -4.0724f), Scale = new Vector3(1.0837f, 0.9680f, 1.1252f), Lift = 0.0085f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(0.0203f, 0.2315f), Euler = new Vector3(4.9566f, 55.7349f, 8.5884f), Scale = new Vector3(1.2769f, 1.1744f, 1.2840f), Lift = 0.0061f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.4780f, 0.1077f), Euler = new Vector3(13.5710f, -0.2172f, -1.2381f), Scale = new Vector3(0.9988f, 0.9347f, 1.0763f), Lift = 0.0064f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(-0.0410f, -0.1205f), Euler = new Vector3(3.2009f, 61.0885f, 1.0076f), Scale = new Vector3(1.2197f, 1.3176f, 1.1379f), Lift = 0.0123f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(0.4298f, -0.1027f), Euler = new Vector3(6.7188f, 84.1888f, -2.6127f), Scale = new Vector3(1.1441f, 0.9571f, 1.0998f), Lift = 0.0082f, Mesh = 2, Juicy = true },
                };
            }

            internal static MeatBit[] Body()
            {
                return new[]
                {
                new MeatBit { Xz = new Vector2(0.0782f, 0.1232f), Euler = new Vector3(-19.2556f, 29.3478f, -1.5167f), Scale = new Vector3(0.5651f, 0.4937f, 0.6033f), Lift = 0.0074f, Juicy = true },
                new MeatBit { Xz = new Vector2(-0.0607f, 0.0017f), Euler = new Vector3(-14.5214f, 19.3970f, 9.1955f), Scale = new Vector3(0.5628f, 0.4492f, 0.5401f), Lift = 0.0012f, Juicy = false },
                new MeatBit { Xz = new Vector2(-0.1950f, -0.2687f), Euler = new Vector3(15.3012f, 39.6973f, -0.9700f), Scale = new Vector3(0.5424f, 0.5336f, 0.5741f), Lift = 0.0048f, Juicy = true },
                new MeatBit { Xz = new Vector2(-0.3008f, 0.1429f), Euler = new Vector3(-19.5959f, 15.0237f, 12.9107f), Scale = new Vector3(0.5067f, 0.5490f, 0.5297f), Lift = 0.0079f, Juicy = false },
                new MeatBit { Xz = new Vector2(0.2226f, -0.0813f), Euler = new Vector3(18.9566f, 81.3360f, 13.7246f), Scale = new Vector3(0.4381f, 0.4862f, 0.4441f), Lift = 0.0022f, Juicy = true },
                new MeatBit { Xz = new Vector2(0.1915f, 0.0939f), Euler = new Vector3(-15.0938f, 68.5499f, -6.2634f), Scale = new Vector3(0.4607f, 0.5021f, 0.4090f), Lift = 0.0016f, Juicy = false },
                };
            }

            internal static StrandBit[] Strands()
            {
                return new[]
                {
                new StrandBit { Xz = new Vector2(-0.0487f, 0.2923f), Euler = new Vector3(-5.4457f, 54.7622f, -10.4172f), Scale = 1.3661f, Lift = 0.0388f, Mesh = 0, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.3668f, -0.1882f), Euler = new Vector3(-10.5685f, 24.2597f, -1.7628f), Scale = 1.2590f, Lift = 0.0256f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1204f, -0.1850f), Euler = new Vector3(-8.9654f, 63.8419f, 3.2105f), Scale = 1.2707f, Lift = 0.0419f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.3460f, -0.0026f), Euler = new Vector3(-3.4138f, 87.2253f, 1.4705f), Scale = 1.4387f, Lift = 0.0335f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.3856f, -0.2874f), Euler = new Vector3(-8.2992f, 19.0234f, 2.3957f), Scale = 1.5361f, Lift = 0.0369f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.5400f, 0.0027f), Euler = new Vector3(9.0711f, 96.3637f, -3.1445f), Scale = 1.4782f, Lift = 0.0213f, Mesh = 2, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.0448f, 0.0766f), Euler = new Vector3(-11.8660f, 52.0978f, 0.2981f), Scale = 1.1060f, Lift = 0.0462f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.3247f, 0.0319f), Euler = new Vector3(6.7901f, 21.7976f, 9.6136f), Scale = 1.5667f, Lift = 0.0376f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1166f, -0.0512f), Euler = new Vector3(-0.3915f, 46.0112f, -7.3737f), Scale = 1.4485f, Lift = 0.0472f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.5151f, 0.1620f), Euler = new Vector3(10.1994f, 74.9476f, 5.2011f), Scale = 1.3256f, Lift = 0.0215f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0793f, -0.4936f), Euler = new Vector3(9.3367f, 6.8714f, 6.8358f), Scale = 1.0543f, Lift = 0.0242f, Mesh = 1, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.1036f, -0.2308f), Euler = new Vector3(1.7877f, 25.3972f, -10.1063f), Scale = 1.2663f, Lift = 0.0453f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0520f, -0.0936f), Euler = new Vector3(7.8478f, 58.1228f, 3.7528f), Scale = 1.2911f, Lift = 0.0581f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.0729f, 0.4595f), Euler = new Vector3(8.9021f, 43.6701f, 11.0706f), Scale = 1.1654f, Lift = 0.0336f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1432f, 0.4401f), Euler = new Vector3(2.6246f, 10.8153f, 4.1952f), Scale = 1.1944f, Lift = 0.0301f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.0581f, -0.5271f), Euler = new Vector3(3.0269f, 54.0498f, -9.4778f), Scale = 1.1907f, Lift = 0.0356f, Mesh = 0, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.1556f, -0.1000f), Euler = new Vector3(1.0596f, 42.0425f, -4.1506f), Scale = 1.5252f, Lift = 0.0484f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0526f, 0.3403f), Euler = new Vector3(-10.7083f, 35.1770f, -8.1351f), Scale = 1.5261f, Lift = 0.0316f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1933f, -0.0284f), Euler = new Vector3(-3.8786f, 74.1287f, -2.7944f), Scale = 1.5442f, Lift = 0.0516f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.0893f, 0.4233f), Euler = new Vector3(-9.8657f, 55.1499f, 11.0691f), Scale = 1.6165f, Lift = 0.0238f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2063f, 0.3749f), Euler = new Vector3(9.9969f, 20.8470f, -9.4769f), Scale = 1.2351f, Lift = 0.0277f, Mesh = 2, Juicy = true },
                new StrandBit { Xz = new Vector2(0.2991f, -0.2213f), Euler = new Vector3(5.3244f, 82.2227f, -1.3920f), Scale = 1.4256f, Lift = 0.0281f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1914f, -0.3983f), Euler = new Vector3(-0.2700f, 59.2051f, -11.8906f), Scale = 1.0775f, Lift = 0.0225f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1526f, 0.0809f), Euler = new Vector3(-0.1635f, 14.8305f, -3.4442f), Scale = 1.4097f, Lift = 0.0397f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.4569f, 0.2792f), Euler = new Vector3(-11.4640f, 17.1970f, 0.4766f), Scale = 1.0894f, Lift = 0.0332f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.2610f, -0.3865f), Euler = new Vector3(-11.7496f, 65.2777f, 8.4780f), Scale = 1.5030f, Lift = 0.0300f, Mesh = 1, Juicy = true },
                new StrandBit { Xz = new Vector2(0.3844f, -0.0703f), Euler = new Vector3(-6.2019f, 87.6407f, -8.4044f), Scale = 1.0533f, Lift = 0.0361f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.4614f, 0.2614f), Euler = new Vector3(-5.1741f, 84.9404f, -6.8216f), Scale = 1.4270f, Lift = 0.0252f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.0833f, -0.2286f), Euler = new Vector3(7.3511f, 61.6593f, 10.3986f), Scale = 1.0923f, Lift = 0.0360f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.0833f, 0.3164f), Euler = new Vector3(3.8889f, 50.2327f, -8.0557f), Scale = 1.0660f, Lift = 0.0376f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1669f, -0.4866f), Euler = new Vector3(-3.9442f, 40.0933f, 8.9818f), Scale = 1.2340f, Lift = 0.0297f, Mesh = 0, Juicy = true },
                new StrandBit { Xz = new Vector2(0.3606f, 0.1746f), Euler = new Vector3(-10.0845f, 96.9184f, -10.6966f), Scale = 1.3199f, Lift = 0.0309f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.3588f, -0.2029f), Euler = new Vector3(1.6189f, 83.7367f, -7.9177f), Scale = 1.6487f, Lift = 0.0355f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.4503f, 0.2981f), Euler = new Vector3(6.3221f, 79.4556f, 2.0740f), Scale = 1.3412f, Lift = 0.0207f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.3104f, -0.0685f), Euler = new Vector3(-6.3434f, 72.0191f, -9.8012f), Scale = 1.5259f, Lift = 0.0283f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2237f, -0.0472f), Euler = new Vector3(11.3149f, 20.2221f, -2.8276f), Scale = 1.3904f, Lift = 0.0483f, Mesh = 2, Juicy = true },
                new StrandBit { Xz = new Vector2(0.0195f, 0.4127f), Euler = new Vector3(4.7331f, 24.1909f, -0.2576f), Scale = 1.4435f, Lift = 0.0360f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.4412f, 0.3113f), Euler = new Vector3(8.7639f, 22.8702f, 9.7791f), Scale = 1.4071f, Lift = 0.0271f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1090f, 0.0934f), Euler = new Vector3(5.3133f, 24.4930f, 7.5853f), Scale = 1.4117f, Lift = 0.0577f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.4076f, -0.1648f), Euler = new Vector3(-4.4830f, 77.6924f, -10.7350f), Scale = 1.4439f, Lift = 0.0371f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.3998f, 0.0463f), Euler = new Vector3(6.4034f, 28.3811f, -8.3798f), Scale = 1.5370f, Lift = 0.0290f, Mesh = 1, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.2928f, 0.1208f), Euler = new Vector3(-5.8998f, 35.2611f, -4.2410f), Scale = 1.2688f, Lift = 0.0440f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1812f, 0.2840f), Euler = new Vector3(11.8695f, 24.8583f, 0.7675f), Scale = 1.2694f, Lift = 0.0336f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.0704f, 0.1434f), Euler = new Vector3(5.5650f, 64.0866f, -6.8893f), Scale = 1.5574f, Lift = 0.0455f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1948f, 0.0956f), Euler = new Vector3(8.4010f, 10.4458f, -3.9305f), Scale = 1.2832f, Lift = 0.0453f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.3042f, 0.0277f), Euler = new Vector3(11.3804f, 64.0880f, -0.0116f), Scale = 1.2322f, Lift = 0.0300f, Mesh = 0, Juicy = true },
                new StrandBit { Xz = new Vector2(0.1355f, -0.3867f), Euler = new Vector3(-5.2163f, 57.1815f, 11.4668f), Scale = 1.6454f, Lift = 0.0266f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.2096f, -0.3000f), Euler = new Vector3(-1.8466f, 67.6617f, 8.2316f), Scale = 1.5263f, Lift = 0.0341f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.4191f, -0.2012f), Euler = new Vector3(-10.6552f, 3.4225f, 1.3017f), Scale = 1.3202f, Lift = 0.0317f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0115f, -0.0559f), Euler = new Vector3(9.1659f, 53.8720f, 10.4781f), Scale = 1.2216f, Lift = 0.0538f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2062f, -0.2709f), Euler = new Vector3(3.0995f, 13.8322f, 2.2978f), Scale = 1.1223f, Lift = 0.0343f, Mesh = 2, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.3643f, 0.2876f), Euler = new Vector3(-4.2296f, 18.2549f, -2.9876f), Scale = 1.3948f, Lift = 0.0246f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.2286f, 0.1693f), Euler = new Vector3(1.3117f, 56.5360f, 11.6579f), Scale = 1.0990f, Lift = 0.0461f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1844f, 0.1604f), Euler = new Vector3(0.0661f, 53.9816f, -1.7553f), Scale = 1.4855f, Lift = 0.0369f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.2559f, 0.1861f), Euler = new Vector3(2.6957f, 91.9814f, 3.6379f), Scale = 1.5306f, Lift = 0.0390f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2571f, -0.1999f), Euler = new Vector3(3.4839f, 10.9621f, -11.1977f), Scale = 1.2629f, Lift = 0.0394f, Mesh = 1, Juicy = true },
                };
            }

            internal static ShredBit[] Shreds()
            {
                return new[]
                {
                new ShredBit { Xz = new Vector2(-0.2467f, -0.2725f), Euler = new Vector3(4.9503f, 18.6493f, 4.2988f), Scale = 1.1807f, Lift = 0.0364f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(0.3305f, -0.2301f), Euler = new Vector3(11.5343f, 87.1918f, 3.2182f), Scale = 1.2695f, Lift = 0.0271f, Mesh = 1 },
                new ShredBit { Xz = new Vector2(0.2395f, -0.4001f), Euler = new Vector3(5.8859f, 59.6763f, 0.3687f), Scale = 1.1573f, Lift = 0.0259f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(0.2722f, -0.1049f), Euler = new Vector3(-8.8081f, 91.1138f, 0.2945f), Scale = 1.4503f, Lift = 0.0406f, Mesh = 1 },
                new ShredBit { Xz = new Vector2(0.0742f, -0.1084f), Euler = new Vector3(-0.7914f, 50.2098f, -6.3781f), Scale = 1.4131f, Lift = 0.0517f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(0.3602f, 0.1781f), Euler = new Vector3(1.3652f, 87.7996f, -8.3674f), Scale = 1.4005f, Lift = 0.0295f, Mesh = 1 },
                new ShredBit { Xz = new Vector2(0.1683f, 0.1770f), Euler = new Vector3(-6.4088f, 68.5357f, -8.9277f), Scale = 1.3380f, Lift = 0.0480f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(-0.1746f, 0.0278f), Euler = new Vector3(5.7834f, 22.5077f, -2.9244f), Scale = 1.3287f, Lift = 0.0472f, Mesh = 1 },
                new ShredBit { Xz = new Vector2(0.4210f, 0.2895f), Euler = new Vector3(-8.0541f, 83.5221f, -3.5681f), Scale = 1.4211f, Lift = 0.0298f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(0.1119f, -0.0489f), Euler = new Vector3(13.6757f, 74.7093f, 6.1106f), Scale = 1.1341f, Lift = 0.0570f, Mesh = 1 },
                };
            }
        }

        internal static class Half
        {
            internal static MatBit[] Mats()
            {
                return new[]
                {
                new MatBit { Xz = new Vector2(0.0000f, 0.0000f), Euler = new Vector3(-5.6887f, 42.7227f, 3.3475f), Scale = new Vector3(1.2200f, 1.4000f, 1.1249f), Lift = 0.0078f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0502f, 0.4074f), Euler = new Vector3(5.6908f, 49.0547f, -2.1709f), Scale = new Vector3(0.9301f, 0.9769f, 0.8943f), Lift = 0.0061f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2706f, 0.3679f), Euler = new Vector3(3.8043f, -5.4624f, 2.5897f), Scale = new Vector3(1.1346f, 0.9492f, 1.0392f), Lift = 0.0095f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(-0.4056f, 0.1483f), Euler = new Vector3(0.3936f, 6.6038f, 0.6734f), Scale = new Vector3(1.1535f, 0.9629f, 1.2409f), Lift = 0.0081f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.3396f, -0.0602f), Euler = new Vector3(1.7481f, 17.2110f, 5.5544f), Scale = new Vector3(1.2246f, 1.0333f, 1.2360f), Lift = 0.0093f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.2807f, -0.3595f), Euler = new Vector3(-0.4840f, 23.5705f, 6.6136f), Scale = new Vector3(0.9286f, 0.9496f, 0.9674f), Lift = 0.0104f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0823f, -0.4156f), Euler = new Vector3(5.8270f, 38.9008f, 4.0422f), Scale = new Vector3(1.0150f, 0.9680f, 1.0142f), Lift = 0.0050f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.1534f, -0.4442f), Euler = new Vector3(-10.8071f, 38.2914f, 9.2275f), Scale = new Vector3(1.0165f, 0.9430f, 0.9173f), Lift = 0.0049f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(0.3091f, -0.2215f), Euler = new Vector3(11.1452f, 76.8042f, -1.0005f), Scale = new Vector3(1.0638f, 1.0003f, 1.1229f), Lift = 0.0074f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(-0.1586f, -0.0246f), Euler = new Vector3(2.0328f, 32.9158f, 5.0949f), Scale = new Vector3(1.1614f, 1.2756f, 1.2259f), Lift = 0.0124f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(0.2901f, -0.2501f), Euler = new Vector3(-9.7458f, 88.3223f, -5.3667f), Scale = new Vector3(1.0218f, 0.9980f, 0.9528f), Lift = 0.0081f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.0992f, -0.0515f), Euler = new Vector3(-5.0536f, 31.7868f, -0.1088f), Scale = new Vector3(1.2328f, 1.3352f, 1.2428f), Lift = 0.0101f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.3644f, -0.2452f), Euler = new Vector3(-5.0777f, 17.7547f, 8.0181f), Scale = new Vector3(1.1576f, 0.9586f, 1.1069f), Lift = 0.0100f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2348f, 0.0924f), Euler = new Vector3(8.3243f, 23.8917f, -5.3126f), Scale = new Vector3(1.0271f, 1.1464f, 0.9282f), Lift = 0.0081f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.1462f, -0.2641f), Euler = new Vector3(-10.3025f, 27.3546f, 2.4166f), Scale = new Vector3(1.1617f, 1.0817f, 1.2413f), Lift = 0.0095f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2406f, 0.0362f), Euler = new Vector3(-7.4469f, 9.9005f, -8.7040f), Scale = new Vector3(1.1381f, 1.1590f, 1.2166f), Lift = 0.0099f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.1054f, -0.0796f), Euler = new Vector3(6.2822f, 52.9056f, -0.7874f), Scale = new Vector3(1.0594f, 1.3119f, 1.0025f), Lift = 0.0083f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2983f, 0.0345f), Euler = new Vector3(2.2626f, 9.1913f, 5.6347f), Scale = new Vector3(1.0290f, 1.0836f, 0.9416f), Lift = 0.0082f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(0.2127f, -0.2212f), Euler = new Vector3(-10.5207f, 65.6664f, 7.8326f), Scale = new Vector3(1.0098f, 1.0756f, 0.9307f), Lift = 0.0110f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.4002f, 0.2828f), Euler = new Vector3(-0.6780f, -9.9750f, -8.7105f), Scale = new Vector3(1.0793f, 0.9347f, 0.9901f), Lift = 0.0054f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(0.3067f, -0.1327f), Euler = new Vector3(-3.8847f, 73.9978f, 1.8308f), Scale = new Vector3(1.0769f, 1.0446f, 1.0188f), Lift = 0.0113f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0127f, -0.2022f), Euler = new Vector3(10.0994f, 30.4880f, 5.6375f), Scale = new Vector3(1.1893f, 1.2169f, 1.2341f), Lift = 0.0095f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.1960f, -0.0928f), Euler = new Vector3(8.5078f, 66.2965f, 6.1611f), Scale = new Vector3(1.0742f, 1.1965f, 1.1380f), Lift = 0.0057f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(0.0541f, -0.1692f), Euler = new Vector3(-4.7048f, 48.2695f, -3.0262f), Scale = new Vector3(1.0233f, 1.2522f, 0.9307f), Lift = 0.0079f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.3562f, -0.1946f), Euler = new Vector3(-1.0042f, 25.7559f, -4.3853f), Scale = new Vector3(0.9431f, 0.9801f, 0.9612f), Lift = 0.0053f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.1328f, 0.3102f), Euler = new Vector3(7.9455f, 37.8448f, 5.1233f), Scale = new Vector3(1.2564f, 1.0411f, 1.1456f), Lift = 0.0083f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(-0.0974f, 0.0078f), Euler = new Vector3(8.4845f, 27.4830f, -8.9318f), Scale = new Vector3(1.2836f, 1.3497f, 1.3375f), Lift = 0.0112f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(0.0736f, -0.4844f), Euler = new Vector3(11.0045f, 49.0607f, 6.8443f), Scale = new Vector3(0.9714f, 0.9347f, 1.0375f), Lift = 0.0103f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2699f, -0.0341f), Euler = new Vector3(8.1289f, 15.1373f, -2.4051f), Scale = new Vector3(1.0270f, 1.1197f, 0.9441f), Lift = 0.0065f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(-0.1193f, 0.1108f), Euler = new Vector3(-3.1667f, 29.7969f, 9.0264f), Scale = new Vector3(1.0688f, 1.2724f, 1.0621f), Lift = 0.0089f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2570f, 0.3564f), Euler = new Vector3(-9.6220f, 9.3943f, 3.2977f), Scale = new Vector3(0.9027f, 0.9585f, 0.9340f), Lift = 0.0041f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.4103f, -0.1061f), Euler = new Vector3(-11.5621f, 29.2612f, 8.2741f), Scale = new Vector3(0.9206f, 0.9680f, 0.9296f), Lift = 0.0097f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.4780f, 0.1077f), Euler = new Vector3(-5.9771f, 9.8630f, 5.6451f), Scale = new Vector3(1.1581f, 0.9347f, 1.0464f), Lift = 0.0092f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(-0.0410f, -0.1205f), Euler = new Vector3(-3.9336f, 46.1338f, -6.9325f), Scale = new Vector3(1.1887f, 1.3176f, 1.2102f), Lift = 0.0124f, Mesh = 1, Juicy = false },
                };
            }

            internal static MeatBit[] Body()
            {
                return new[]
                {
                new MeatBit { Xz = new Vector2(-0.0607f, 0.0017f), Euler = new Vector3(-19.2556f, 13.7197f, -1.5167f), Scale = new Vector3(0.5651f, 0.4937f, 0.6033f), Lift = 0.0074f, Juicy = false },
                new MeatBit { Xz = new Vector2(-0.1950f, -0.2687f), Euler = new Vector3(-14.5214f, -0.6311f, 9.1955f), Scale = new Vector3(0.5628f, 0.4492f, 0.5401f), Lift = 0.0012f, Juicy = true },
                new MeatBit { Xz = new Vector2(-0.3008f, 0.1429f), Euler = new Vector3(15.3012f, 34.3666f, -0.9700f), Scale = new Vector3(0.5424f, 0.5336f, 0.5741f), Lift = 0.0048f, Juicy = false },
                new MeatBit { Xz = new Vector2(0.2226f, -0.0813f), Euler = new Vector3(-19.5959f, 72.0581f, 12.9107f), Scale = new Vector3(0.5067f, 0.5490f, 0.5297f), Lift = 0.0079f, Juicy = true },
                };
            }

            internal static StrandBit[] Strands()
            {
                return new[]
                {
                new StrandBit { Xz = new Vector2(-0.0487f, 0.2923f), Euler = new Vector3(-5.4457f, 54.7622f, -10.4172f), Scale = 1.3661f, Lift = 0.0388f, Mesh = 0, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.3668f, -0.1882f), Euler = new Vector3(-10.5685f, 24.2597f, -1.7628f), Scale = 1.2590f, Lift = 0.0256f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1204f, -0.1850f), Euler = new Vector3(-8.9654f, 63.8419f, 3.2105f), Scale = 1.2707f, Lift = 0.0419f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.3856f, -0.2874f), Euler = new Vector3(-3.4138f, 11.5090f, 1.4705f), Scale = 1.4387f, Lift = 0.0282f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0448f, 0.0766f), Euler = new Vector3(-8.2992f, 54.8896f, 2.3957f), Scale = 1.5361f, Lift = 0.0608f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.3247f, 0.0319f), Euler = new Vector3(9.0711f, 26.0049f, -3.1445f), Scale = 1.4782f, Lift = 0.0289f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1166f, -0.0512f), Euler = new Vector3(-11.8660f, 71.6143f, 0.2981f), Scale = 1.1060f, Lift = 0.0439f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0793f, -0.4936f), Euler = new Vector3(6.7901f, 29.3270f, 9.6136f), Scale = 1.5667f, Lift = 0.0306f, Mesh = 1, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.1036f, -0.2308f), Euler = new Vector3(-0.3915f, 16.1476f, -7.3737f), Scale = 1.4485f, Lift = 0.0374f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0520f, -0.0936f), Euler = new Vector3(10.1994f, 32.0364f, 5.2011f), Scale = 1.3256f, Lift = 0.0455f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1432f, 0.4401f), Euler = new Vector3(9.3367f, 2.4569f, 6.8358f), Scale = 1.0543f, Lift = 0.0250f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.0581f, -0.5271f), Euler = new Vector3(1.7877f, 31.0079f, -10.1063f), Scale = 1.2663f, Lift = 0.0325f, Mesh = 0, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.1556f, -0.1000f), Euler = new Vector3(7.8478f, 46.1792f, 3.7528f), Scale = 1.2911f, Lift = 0.0525f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0526f, 0.3403f), Euler = new Vector3(8.9021f, 34.3683f, 11.0706f), Scale = 1.1654f, Lift = 0.0387f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2063f, 0.3749f), Euler = new Vector3(2.6246f, 7.5740f, 4.1952f), Scale = 1.1944f, Lift = 0.0312f, Mesh = 2, Juicy = true },
                new StrandBit { Xz = new Vector2(0.2991f, -0.2213f), Euler = new Vector3(3.0269f, 92.1708f, -9.4778f), Scale = 1.1907f, Lift = 0.0405f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1914f, -0.3983f), Euler = new Vector3(1.0596f, 72.0033f, -4.1506f), Scale = 1.5252f, Lift = 0.0321f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1526f, 0.0809f), Euler = new Vector3(-10.7083f, 31.3014f, -8.1351f), Scale = 1.5261f, Lift = 0.0445f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.4569f, 0.2792f), Euler = new Vector3(-3.8786f, 8.8313f, -2.7944f), Scale = 1.5442f, Lift = 0.0342f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(0.2610f, -0.3865f), Euler = new Vector3(-9.8657f, 74.4377f, 11.0691f), Scale = 1.6165f, Lift = 0.0228f, Mesh = 1, Juicy = true },
                new StrandBit { Xz = new Vector2(0.0833f, -0.2286f), Euler = new Vector3(9.9969f, 60.0391f, -9.4769f), Scale = 1.2351f, Lift = 0.0387f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1669f, -0.4866f), Euler = new Vector3(5.3244f, 58.6594f, -1.3920f), Scale = 1.4256f, Lift = 0.0235f, Mesh = 0, Juicy = true },
                new StrandBit { Xz = new Vector2(0.3588f, -0.2029f), Euler = new Vector3(-0.2700f, 79.8016f, -11.8906f), Scale = 1.0775f, Lift = 0.0236f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2237f, -0.0472f), Euler = new Vector3(-0.1635f, 8.0397f, -3.4442f), Scale = 1.4097f, Lift = 0.0352f, Mesh = 2, Juicy = true },
                new StrandBit { Xz = new Vector2(0.0195f, 0.4127f), Euler = new Vector3(-11.4640f, 51.3962f, 0.4766f), Scale = 1.0894f, Lift = 0.0363f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.4412f, 0.3113f), Euler = new Vector3(-11.7496f, 2.9689f, 8.4780f), Scale = 1.5030f, Lift = 0.0286f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1090f, 0.0934f), Euler = new Vector3(-6.2019f, 38.5606f, -8.4044f), Scale = 1.0533f, Lift = 0.0535f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.4076f, -0.1648f), Euler = new Vector3(-5.1741f, 86.6676f, -6.8216f), Scale = 1.4270f, Lift = 0.0272f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.3998f, 0.0463f), Euler = new Vector3(7.3511f, 18.8640f, 10.3986f), Scale = 1.0923f, Lift = 0.0259f, Mesh = 1, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.2928f, 0.1208f), Euler = new Vector3(3.8889f, 15.5583f, -8.0557f), Scale = 1.0660f, Lift = 0.0382f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1812f, 0.2840f), Euler = new Vector3(-3.9442f, 9.5841f, 8.9818f), Scale = 1.2340f, Lift = 0.0363f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1948f, 0.0956f), Euler = new Vector3(-10.0845f, 41.5458f, -10.6966f), Scale = 1.3199f, Lift = 0.0430f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1355f, -0.3867f), Euler = new Vector3(1.6189f, 57.8205f, -7.9177f), Scale = 1.6487f, Lift = 0.0356f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.2096f, -0.3000f), Euler = new Vector3(6.3221f, 65.3139f, 2.0740f), Scale = 1.3412f, Lift = 0.0260f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.4191f, -0.2012f), Euler = new Vector3(-6.3434f, 0.5826f, -9.8012f), Scale = 1.5259f, Lift = 0.0216f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0115f, -0.0559f), Euler = new Vector3(11.3149f, 43.6980f, -2.8276f), Scale = 1.3904f, Lift = 0.0598f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2062f, -0.2709f), Euler = new Vector3(4.7331f, 4.7923f, -0.2576f), Scale = 1.4435f, Lift = 0.0396f, Mesh = 2, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.3643f, 0.2876f), Euler = new Vector3(8.7639f, 26.4170f, 9.7791f), Scale = 1.4071f, Lift = 0.0285f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2571f, -0.1999f), Euler = new Vector3(5.3133f, 7.3632f, 7.5853f), Scale = 1.4117f, Lift = 0.0438f, Mesh = 1, Juicy = true },
                };
            }

            internal static ShredBit[] Shreds()
            {
                return new[]
                {
                new ShredBit { Xz = new Vector2(-0.2467f, -0.2725f), Euler = new Vector3(4.9503f, 18.6493f, 4.2988f), Scale = 1.1807f, Lift = 0.0364f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(0.3305f, -0.2301f), Euler = new Vector3(11.5343f, 87.1918f, 3.2182f), Scale = 1.2695f, Lift = 0.0271f, Mesh = 1 },
                new ShredBit { Xz = new Vector2(0.2395f, -0.4001f), Euler = new Vector3(5.8859f, 59.6763f, 0.3687f), Scale = 1.1573f, Lift = 0.0259f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(0.2722f, -0.1049f), Euler = new Vector3(-8.8081f, 91.1138f, 0.2945f), Scale = 1.4503f, Lift = 0.0406f, Mesh = 1 },
                new ShredBit { Xz = new Vector2(0.0742f, -0.1084f), Euler = new Vector3(-0.7914f, 50.2098f, -6.3781f), Scale = 1.4131f, Lift = 0.0517f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(-0.1746f, 0.0278f), Euler = new Vector3(1.3652f, 35.1844f, -8.3674f), Scale = 1.4005f, Lift = 0.0465f, Mesh = 1 },
                new ShredBit { Xz = new Vector2(0.1119f, -0.0489f), Euler = new Vector3(-6.4088f, 64.4717f, -8.9277f), Scale = 1.3380f, Lift = 0.0585f, Mesh = 1 },
                };
            }
        }

        internal static class Low
        {
            internal static MatBit[] Mats()
            {
                return new[]
                {
                new MatBit { Xz = new Vector2(0.0000f, 0.0000f), Euler = new Vector3(-5.6887f, 42.7227f, 3.3475f), Scale = new Vector3(1.2200f, 1.4000f, 1.1249f), Lift = 0.0078f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.1586f, -0.0246f), Euler = new Vector3(5.6908f, 47.8525f, -2.1709f), Scale = new Vector3(1.0335f, 1.2756f, 0.9937f), Lift = 0.0083f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0992f, -0.0515f), Euler = new Vector3(3.8043f, 20.7461f, 2.5897f), Scale = new Vector3(1.2607f, 1.3352f, 1.1546f), Lift = 0.0124f, Mesh = 2, Juicy = false },
                new MatBit { Xz = new Vector2(0.0415f, 0.0541f), Euler = new Vector3(0.3936f, 49.6281f, 0.6734f), Scale = new Vector3(1.2817f, 1.3749f, 1.3788f), Lift = 0.0112f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.0506f, 0.1106f), Euler = new Vector3(1.7481f, 56.3358f, 5.5544f), Scale = new Vector3(1.2246f, 1.3242f, 1.2360f), Lift = 0.0115f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(-0.2348f, 0.0924f), Euler = new Vector3(-0.4840f, 35.6477f, 6.6136f), Scale = new Vector3(1.0318f, 1.1464f, 1.0748f), Lift = 0.0119f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.2406f, 0.0362f), Euler = new Vector3(5.8270f, 33.8909f, 4.0422f), Scale = new Vector3(1.1278f, 1.1590f, 1.1269f), Lift = 0.0065f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.1054f, -0.0796f), Euler = new Vector3(-10.8071f, 45.5977f, 9.2275f), Scale = new Vector3(1.1295f, 1.3119f, 1.0192f), Lift = 0.0076f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.2983f, 0.0345f), Euler = new Vector3(11.1452f, 17.5034f, -1.0005f), Scale = new Vector3(1.0638f, 1.0836f, 1.1229f), Lift = 0.0080f, Mesh = 2, Juicy = true },
                new MatBit { Xz = new Vector2(0.2127f, -0.2212f), Euler = new Vector3(2.0328f, 71.7984f, 5.0949f), Scale = new Vector3(1.1614f, 1.0756f, 1.2259f), Lift = 0.0109f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0127f, -0.2022f), Euler = new Vector3(-9.7458f, 57.8734f, -5.3667f), Scale = new Vector3(1.0218f, 1.2169f, 0.9528f), Lift = 0.0097f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(0.1960f, -0.0928f), Euler = new Vector3(-5.0536f, 65.5061f, -0.1088f), Scale = new Vector3(1.2328f, 1.1965f, 1.2428f), Lift = 0.0091f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(0.0541f, -0.1692f), Euler = new Vector3(-5.0777f, 61.4666f, 8.0181f), Scale = new Vector3(1.2862f, 1.2522f, 1.2299f), Lift = 0.0122f, Mesh = 0, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0974f, 0.0078f), Euler = new Vector3(8.3243f, 38.8129f, -5.3126f), Scale = new Vector3(1.0271f, 1.3497f, 0.9282f), Lift = 0.0096f, Mesh = 1, Juicy = true },
                new MatBit { Xz = new Vector2(-0.2699f, -0.0341f), Euler = new Vector3(-10.3025f, 20.5011f, 2.4166f), Scale = new Vector3(1.1617f, 1.1197f, 1.2413f), Lift = 0.0098f, Mesh = 0, Juicy = true },
                new MatBit { Xz = new Vector2(-0.1193f, 0.1108f), Euler = new Vector3(-7.4469f, 21.3827f, -8.7040f), Scale = new Vector3(1.1381f, 1.2724f, 1.2166f), Lift = 0.0108f, Mesh = 1, Juicy = false },
                new MatBit { Xz = new Vector2(-0.0410f, -0.1205f), Euler = new Vector3(6.2822f, 34.7492f, -0.7874f), Scale = new Vector3(1.0594f, 1.3176f, 1.0025f), Lift = 0.0083f, Mesh = 1, Juicy = false },
                };
            }

            internal static MeatBit[] Body()
            {
                return new[]
                {
                new MeatBit { Xz = new Vector2(-0.0607f, 0.0017f), Euler = new Vector3(-19.2556f, 13.7197f, -1.5167f), Scale = new Vector3(0.5651f, 0.4937f, 0.6033f), Lift = 0.0074f, Juicy = false },
                new MeatBit { Xz = new Vector2(-0.3008f, 0.1429f), Euler = new Vector3(-14.5214f, -5.9618f, 9.1955f), Scale = new Vector3(0.5628f, 0.4492f, 0.5401f), Lift = 0.0012f, Juicy = false },
                };
            }

            internal static StrandBit[] Strands()
            {
                return new[]
                {
                new StrandBit { Xz = new Vector2(0.1204f, -0.1850f), Euler = new Vector3(-5.4457f, 78.4964f, -10.4172f), Scale = 1.3661f, Lift = 0.0446f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0448f, 0.0766f), Euler = new Vector3(-10.5685f, 56.0246f, -1.7628f), Scale = 1.2590f, Lift = 0.0474f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.3247f, 0.0319f), Euler = new Vector3(-8.9654f, 19.2293f, 3.2105f), Scale = 1.2707f, Lift = 0.0340f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1166f, -0.0512f), Euler = new Vector3(-3.4138f, 66.8916f, 1.4705f), Scale = 1.4387f, Lift = 0.0498f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0520f, -0.0936f), Euler = new Vector3(-8.2992f, 53.8021f, 2.3957f), Scale = 1.5361f, Lift = 0.0598f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1556f, -0.1000f), Euler = new Vector3(9.0711f, 39.9415f, -3.1445f), Scale = 1.4782f, Lift = 0.0396f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1526f, 0.0809f), Euler = new Vector3(-11.8660f, 39.6489f, 0.2981f), Scale = 1.1060f, Lift = 0.0406f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.0833f, -0.2286f), Euler = new Vector3(6.7901f, 60.8633f, 9.6136f), Scale = 1.5667f, Lift = 0.0436f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2237f, -0.0472f), Euler = new Vector3(-0.3915f, 7.2550f, -7.3737f), Scale = 1.4485f, Lift = 0.0394f, Mesh = 2, Juicy = true },
                new StrandBit { Xz = new Vector2(-0.1090f, 0.0934f), Euler = new Vector3(10.1994f, 25.3529f, 5.2011f), Scale = 1.3256f, Lift = 0.0431f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.2928f, 0.1208f), Euler = new Vector3(9.3367f, 0.4771f, 6.8358f), Scale = 1.0543f, Lift = 0.0317f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1812f, 0.2840f), Euler = new Vector3(1.7877f, 15.0562f, -10.1063f), Scale = 1.2663f, Lift = 0.0394f, Mesh = 0, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.1948f, 0.0956f), Euler = new Vector3(7.8478f, 42.2136f, 3.7528f), Scale = 1.2911f, Lift = 0.0499f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(0.1355f, -0.3867f), Euler = new Vector3(8.9021f, 54.5515f, 11.0706f), Scale = 1.1654f, Lift = 0.0355f, Mesh = 1, Juicy = false },
                new StrandBit { Xz = new Vector2(0.2096f, -0.3000f), Euler = new Vector3(2.6246f, 57.7825f, 4.1952f), Scale = 1.1944f, Lift = 0.0339f, Mesh = 2, Juicy = false },
                new StrandBit { Xz = new Vector2(-0.0115f, -0.0559f), Euler = new Vector3(3.0269f, 63.0225f, -9.4778f), Scale = 1.1907f, Lift = 0.0619f, Mesh = 1, Juicy = false },
                };
            }

            internal static ShredBit[] Shreds()
            {
                return new[]
                {
                new ShredBit { Xz = new Vector2(0.0742f, -0.1084f), Euler = new Vector3(4.9503f, 58.9948f, 4.2988f), Scale = 1.1807f, Lift = 0.0554f, Mesh = 0 },
                new ShredBit { Xz = new Vector2(-0.1746f, 0.0278f), Euler = new Vector3(11.5343f, 37.7055f, 3.2182f), Scale = 1.2695f, Lift = 0.0441f, Mesh = 1 },
                new ShredBit { Xz = new Vector2(0.1119f, -0.0489f), Euler = new Vector3(5.8859f, 57.3308f, 0.3687f), Scale = 1.1573f, Lift = 0.0497f, Mesh = 1 },
                };
            }
        }

        internal static class LayoutVeg
        {
            internal static VegBit[] Full()
            {
                return new[]
                {
            new VegBit { Kind = 0, Xz = new Vector2(0.2000f, 0.1200f), Euler = new Vector3(18.0000f, 40.0000f, 12.0000f), Scale = 0.2000f },
            new VegBit { Kind = 1, Xz = new Vector2(-0.1600f, 0.2200f), Euler = new Vector3(22.0000f, 120.0000f, -8.0000f), Scale = 0.1800f },
            new VegBit { Kind = 0, Xz = new Vector2(0.1000f, -0.2800f), Euler = new Vector3(14.0000f, 200.0000f, 16.0000f), Scale = 0.2100f },
            new VegBit { Kind = 1, Xz = new Vector2(0.2200f, -0.2000f), Euler = new Vector3(28.0000f, 230.0000f, -6.0000f), Scale = 0.1700f },
            new VegBit { Kind = 0, Xz = new Vector2(-0.2800f, -0.1000f), Euler = new Vector3(16.0000f, 300.0000f, 10.0000f), Scale = 0.1900f },
            new VegBit { Kind = 1, Xz = new Vector2(0.0400f, 0.3000f), Euler = new Vector3(24.0000f, 80.0000f, 8.0000f), Scale = 0.1600f },
            new VegBit { Kind = 2, Xz = new Vector2(0.2600f, 0.0000f), Euler = new Vector3(48.0000f, 20.0000f, 14.0000f), Scale = 0.6200f },
            new VegBit { Kind = 2, Xz = new Vector2(-0.0800f, -0.2400f), Euler = new Vector3(55.0000f, 160.0000f, -10.0000f), Scale = 0.5800f },
            new VegBit { Kind = 2, Xz = new Vector2(-0.2200f, 0.0800f), Euler = new Vector3(42.0000f, 250.0000f, 18.0000f), Scale = 0.6400f },
            new VegBit { Kind = 2, Xz = new Vector2(0.1200f, 0.2200f), Euler = new Vector3(60.0000f, 100.0000f, -8.0000f), Scale = 0.5500f },
            new VegBit { Kind = 2, Xz = new Vector2(0.0000f, -0.0800f), Euler = new Vector3(50.0000f, 200.0000f, 12.0000f), Scale = 0.5000f },
                };
            }

            internal static VegBit[] Half()
            {
                return new[]
                {
            new VegBit { Kind = 0, Xz = new Vector2(-0.1600f, 0.2200f), Euler = new Vector3(22.0000f, 120.0000f, -8.0000f), Scale = 0.1800f },
            new VegBit { Kind = 1, Xz = new Vector2(0.1000f, -0.2800f), Euler = new Vector3(14.0000f, 200.0000f, 16.0000f), Scale = 0.2000f },
            new VegBit { Kind = 0, Xz = new Vector2(-0.2800f, -0.1000f), Euler = new Vector3(16.0000f, 300.0000f, 10.0000f), Scale = 0.1800f },
            new VegBit { Kind = 2, Xz = new Vector2(-0.0800f, -0.2400f), Euler = new Vector3(55.0000f, 160.0000f, -10.0000f), Scale = 0.5800f },
            new VegBit { Kind = 2, Xz = new Vector2(-0.2200f, 0.0800f), Euler = new Vector3(42.0000f, 250.0000f, 18.0000f), Scale = 0.6200f },
            new VegBit { Kind = 2, Xz = new Vector2(0.0000f, -0.0800f), Euler = new Vector3(50.0000f, 200.0000f, 12.0000f), Scale = 0.5000f },
                };
            }

            internal static VegBit[] Low()
            {
                return new[]
                {
            new VegBit { Kind = 0, Xz = new Vector2(-0.1600f, 0.2200f), Euler = new Vector3(22.0000f, 120.0000f, -8.0000f), Scale = 0.1700f },
            new VegBit { Kind = 1, Xz = new Vector2(0.0000f, -0.0800f), Euler = new Vector3(18.0000f, 210.0000f, 8.0000f), Scale = 0.1600f },
            new VegBit { Kind = 2, Xz = new Vector2(-0.2200f, 0.0800f), Euler = new Vector3(42.0000f, 250.0000f, 18.0000f), Scale = 0.5600f },
                };
            }
        }
    }
