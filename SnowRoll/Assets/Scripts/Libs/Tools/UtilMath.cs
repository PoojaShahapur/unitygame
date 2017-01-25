using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public enum AngleUnit
    {
        AU_DEGREE,
        AU_RADIAN
    }

    public class UtilMath
    {
        public const float PI = UnityEngine.Mathf.PI;
        public const float TWO_PI = (float)2.0 * UnityEngine.Mathf.PI;
        public const float HALF_PI = (float)(0.5f * PI);
        public const float fDeg2Rad = (float)(PI / 180.0f);
        public const float fRad2Deg = (float)(180.0f / PI);

        public const float POS_INFINITY = float.PositiveInfinity;
        public const float NEG_INFINITY = float.NegativeInfinity;
        public const float EPSILON = 1e-6f;
        public static float LOG2 = Mathf.Log((float)(2.0));

        public static AngleUnit msAngleUnit;
        public static int mTrigTableSize;
        public static float mTrigTableFactor;
        public static float[] mSinTable;
        public static float[] mTanTable;

        public static Vector3 ZeroVec3 = Vector3.zero;
        public static Quaternion UnitQuat = new Quaternion(0, 0, 0, 1);

        public static bool mIsLimitXRotate = true;     // 是否显示 X 轴旋转
        public static bool mIsLimitYRotate = false;     // 是否显示 Y 轴旋转
        public static bool mIsLimitZRotate = true;     // 是否显示 Z 轴旋转

        public UtilMath( int trigTableSize )
        {
            msAngleUnit = AngleUnit.AU_DEGREE;
            mTrigTableSize = trigTableSize;
            mTrigTableFactor = (float)(mTrigTableSize / TWO_PI);

            mSinTable = new float[mTrigTableSize];
            mTanTable = new float[mTrigTableSize];

            buildTrigTables();
        }

        static public void buildTrigTables()
        {
            float angle;
            for (int i = 0; i < mTrigTableSize; ++i)
            {
                angle = (float)((TWO_PI * i) / mTrigTableSize);
                mSinTable[i] = sin(angle);
                mTanTable[i] = tan(angle);
            }
        }

        static public float SinTable(float fValue)
        {
            // Convert range to index values, wrap if required
            int idx;
            if (fValue >= 0)
            {
                idx = (int)(fValue * mTrigTableFactor) % mTrigTableSize;
            }
            else
            {
                idx = mTrigTableSize - ((int)(-fValue * mTrigTableFactor) % mTrigTableSize) - 1;
            }

            return mSinTable[idx];
        }

        static public float TanTable(float fValue)
        {
            // Convert range to index values, wrap if required
            int idx = (int)(fValue *= mTrigTableFactor) % mTrigTableSize;
            return mTanTable[idx];
        }

        static public bool isVisible(Camera camera, MRectangleF box)
        {
            return false;
        }

        // 设置状态为
        static public void setState(int stateID, params byte[] stateBytes)
        {
            if((int)stateID/8 < stateBytes.Length)
            {
                stateBytes[(int)stateID/8] |= ((byte)(1 << ((int)stateID % 8)));
            }
        }

        static public void clearState(int stateID, params byte[] stateBytes)
        {
            if ((int)stateID/8 < stateBytes.Length)
            {
                stateBytes[(int)stateID / 8] &= (byte)(~((byte)(1 << ((int)stateID % 8))));
            }
        }

        static public bool checkState(int stateID, params byte[] stateBytes)
        {
            if((int)stateID/8 < stateBytes.Length)
            {
                return ((stateBytes[(int)stateID / 8] & ((byte)(1 << ((int)stateID % 8)))) > 0);
            }

            return false;
        }

        static public void clearState(int stateID, uint state)
        {
            if ((uint)stateID < uint.MaxValue)
            {
                state &= (byte)(~((byte)(1 << ((int)stateID % 8))));
            }
        }

        static public bool checkAttackState(int stateID, uint state)
        {
            if ((uint)stateID < uint.MaxValue)
            {
                return ((state & (uint)stateID) > 0);
            }

            return false;
        }

        static public void setState(int idx, ref byte stateData)
        {
            if (idx < sizeof(byte) * 8)
            {
                stateData |= ((byte)(1 << idx));
            }
        }

        static public void clearState(int idx, ref byte stateData)
        {
            if (idx < sizeof(byte) * 8)
            {
                stateData &= ((byte)(~(1 << idx)));
            }
        }

        static public bool checkState(int idx, byte stateData)
        {
            if (idx < sizeof(byte) * 8)
            {
                return ((stateData & (1 << idx)) > 0);
            }

            return false;
        }

        /**
         * @brief 转换一个 Color 值到一个 Int 颜色值
         */
        static public int ColorToInt32(Color c)
        {
            int retVal = 0;
            retVal |= Mathf.RoundToInt(c.r * 255f) << 24;
            retVal |= Mathf.RoundToInt(c.g * 255f) << 16;
            retVal |= Mathf.RoundToInt(c.b * 255f) << 8;
            retVal |= Mathf.RoundToInt(c.a * 255f);
            return retVal;
        }

        /**
         * @brief 转换一个 Int R， G， B， A 值到 color 值
         */
        static public Color Int32ToColor(int val)
        {
            float inv = 1f / 255f;
            Color c = Color.black;
            c.r = inv * ((val >> 24) & 0xFF);
            c.g = inv * ((val >> 16) & 0xFF);
            c.b = inv * ((val >> 8) & 0xFF);
            c.a = inv * (val & 0xFF);
            return c;
        }

        /**
         * @brief 转换 Int R, G, B 值成 Color
         */
        static public Color Int24ToColor(int val)
        {
            float inv = 1f / 255f;
            Color c = Color.black;
            c.r = inv * ((val >> 16) & 0xFF);
            c.g = inv * ((val >> 8) & 0xFF);
            c.b = inv * (val & 0xFF);
            c.a = 1.0f;
            return c;
        }

        static public float Sqrt(float d)
        {
            return UnityEngine.Mathf.Sqrt(d);
        }

        /**
         * @brief 两个整数除，获取 float 值
         * @param numerator 分子
         * @param denominator 分母
         */
        static public float div(int numerator, int denominator)
        {
            return ((float)numerator) / denominator;
        }

        /**
         * @brief tan
         */
        static public float tan(float a)
        {
            return (float)(UnityEngine.Mathf.Tan(a));
        }

        /**
         * @brief atan
         */
        static public float atan(float a)
        {
            return (float)(UnityEngine.Mathf.Atan(a));
        }

        static public int powerTwo(int exponent)
        {
            return (int)UnityEngine.Mathf.Pow(2, exponent);
        }

        static public float Abs(float value)
        {
            return UnityEngine.Mathf.Abs(value);
        }

        static public void swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        static public void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        static public float min(float a, float b)
        {
            return Mathf.Min(a, b);
        }

        static public float max(float a, float b)
        {
            return Mathf.Max(a, b);
        }

        static public long min(long a, long b)
        {
            return (long)Mathf.Min(a, b);
        }

        static public long max(long a, long b)
        {
            return (long)Mathf.Max(a, b);
        }

        static public float ACos(float f)
        {
            return Mathf.Acos(f);
        }

        static public bool RealEqual(float a, float b, float tolerance = 1e-04f)
        {
            if (Mathf.Abs(b - a) <= tolerance)
                return true;
            else
                return false;
        }

        static public float UnitRandom()
        {
            //UnityEngine.Random.seed = (int)UtilApi.getUTCSec();
            UnityEngine.Random.InitState((int)UtilApi.getUTCSec());
            return UnityEngine.Random.Range(0, float.MaxValue) / float.MaxValue;
        }

        public static int RangeRandom(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        // 在单位圆内随机一个点
        static public UnityEngine.Vector3 UnitCircleRandom()
        {
            UnityEngine.Vector3 orient = new UnityEngine.Vector3(
                UtilMath.UnitRandom(), 
                UtilMath.UnitRandom(), 
                UtilMath.UnitRandom());

            orient.Normalize();

            return orient;
        }

        static public float Clamp(float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        static public float Sin(MRadian fValue, bool useTables = false)
        {
            return (!useTables) ? (float)(Mathf.Sin(fValue.valueRadians())) : SinTable(fValue.valueRadians());
        }

        static public float Sin(float fValue, bool useTables = false)
        {
            return (!useTables) ? (float)(sin(fValue)) : SinTable(fValue);
        }

        static public float Cos(MRadian fValue, bool useTables = false)
        {
            return (!useTables) ? (float)(cos(fValue.valueRadians())) : SinTable(fValue.valueRadians() + HALF_PI);
        }

        static public float Cos(float fValue, bool useTables = false)
        {
            return (!useTables) ? (float)(cos(fValue)) : SinTable(fValue + HALF_PI);
        }

        static public float Tan(MRadian fValue, bool useTables = false) {
            return (!useTables) ? (float)(tan(fValue.valueRadians())) : TanTable(fValue.valueRadians());
        }

        static public float Tan(float fValue, bool useTables = false)
        {
            return (!useTables) ? (float)(tan(fValue)) : TanTable(fValue);
        }

        static public float sin(float f)
        {
            return Mathf.Sin(f);
        }

        //static public float Sin(float f)
        //{
        //    return Mathf.Sin(f);
        //}

        static public float cos(float f)
        {
            return Mathf.Cos(f);
        }

        //static public float Cos(float f)
        //{
        //    return Mathf.Cos(f);
        //}

        static public float InvSqrt(float fValue)
        {
            return 1 / Mathf.Sqrt(fValue);
        }

        static public float ATan2(float y, float x)
        {
            return Mathf.Atan2(y, x);
        }

        static public float ASin(float f)
        {
            return Mathf.Asin(f);
        }

        static public float Sqr(float fValue)
        {
            return fValue * fValue;
        }

        static public bool intersects(MPlane plane, MAxisAlignedBox box)
        {
            return (plane.getSide(ref box) == MPlane.Side.BOTH_SIDE);
        }

        static public float Sign(float fValue)
        {
            if (fValue > 0.0)
                return 1.0f;

            if (fValue < 0.0)
                return -1.0f;

            return 0.0f;
        }

        static public MMatrix4 buildReflectionMatrix(ref MPlane p)
        {
            return new MMatrix4(
                -2 * p.normal.x * p.normal.x + 1, -2 * p.normal.x * p.normal.y, -2 * p.normal.x * p.normal.z, -2 * p.normal.x * p.d,
                -2 * p.normal.y * p.normal.x, -2 * p.normal.y * p.normal.y + 1, -2 * p.normal.y * p.normal.z, -2 * p.normal.y * p.d,
                -2 * p.normal.z * p.normal.x, -2 * p.normal.z * p.normal.y, -2 * p.normal.z * p.normal.z + 1, -2 * p.normal.z * p.d,
                0, 0, 0, 1);
        }

        static public MMatrix4 makeViewMatrix(ref MVector3 position, ref MQuaternion orientation, ref MMatrix4 reflectMatrix, bool reflect)
        {
            MMatrix4 viewMatrix = new MMatrix4(0);

            MMatrix3 rot = new MMatrix3(0);
            orientation.ToRotationMatrix(ref rot);

            MMatrix3 rotT = rot.Transpose();
            MVector3 trans = -rotT * position;

            viewMatrix = MMatrix4.IDENTITY;
            viewMatrix.assignForm(ref rotT);
            viewMatrix[0, 3] = trans.x;
            viewMatrix[1, 3] = trans.y;
            viewMatrix[2, 3] = trans.z;

            viewMatrix[2, 0] = -viewMatrix[2, 0];
            viewMatrix[2, 1] = -viewMatrix[2, 1];
            viewMatrix[2, 2] = -viewMatrix[2, 2];
            viewMatrix[2, 3] = -viewMatrix[2, 3];

            if (reflect && reflectMatrix != null)
            {
                viewMatrix = viewMatrix * (reflectMatrix);
            }

            return viewMatrix;
        }

        static public MKeyValuePair<bool, float> intersects(MRay ray, MPlane plane)
        {
            MVector3 dir = ray.getDirection();
            float denom = plane.normal.dotProduct(ref dir);
            if (UtilMath.Abs(denom) < EPSILON)
            {
                return new MKeyValuePair<bool, float>(false, (float)0);
            }
            else
            {
                MVector3 orig = ray.getOrigin();
                float nom = plane.normal.dotProduct(ref orig) + plane.d;
                float t = -(nom / denom);
                return new MKeyValuePair<bool, float>(t >= 0, (float)t);
            }
        }

        static public KeyValuePair<bool, float> intersects(MRay ray, ref MAxisAlignedBox box)
        {
            if (box.isNull()) return new KeyValuePair<bool, float>(false, (float)0);
            if (box.isInfinite()) return new KeyValuePair<bool, float>(true, (float)0);

            float lowt = 0.0f;
            float t;
            bool hit = false;
            MVector3 hitpoint;
            MVector3 min = box.getMinimum();
            MVector3 max = box.getMaximum();
            MVector3 rayorig = ray.getOrigin();
            MVector3 raydir = ray.getDirection();

            if (rayorig > min && rayorig < max)
            {
                return new KeyValuePair<bool, float>(true, (float)0);
            }

            if (rayorig.x <= min.x && raydir.x > 0)
            {
                t = (min.x - rayorig.x) / raydir.x;
                if (t >= 0)
                {
                    hitpoint = rayorig + raydir * t;
                    if (hitpoint.y >= min.y && hitpoint.y <= max.y &&
                        hitpoint.z >= min.z && hitpoint.z <= max.z &&
                        (!hit || t < lowt))
                    {
                        hit = true;
                        lowt = t;
                    }
                }
            }

            if (rayorig.x >= max.x && raydir.x < 0)
            {
                t = (max.x - rayorig.x) / raydir.x;

                hitpoint = rayorig + raydir * t;
                if (hitpoint.y >= min.y && hitpoint.y <= max.y &&
                    hitpoint.z >= min.z && hitpoint.z <= max.z &&
                    (!hit || t < lowt))
                {
                    hit = true;
                    lowt = t;
                }
            }

            if (rayorig.y <= min.y && raydir.y > 0)
            {
                t = (min.y - rayorig.y) / raydir.y;

                hitpoint = rayorig + raydir * t;
                if (hitpoint.x >= min.x && hitpoint.x <= max.x &&
                    hitpoint.z >= min.z && hitpoint.z <= max.z &&
                    (!hit || t < lowt))
                {
                    hit = true;
                    lowt = t;
                }
            }

            if (rayorig.y >= max.y && raydir.y < 0)
            {
                t = (max.y - rayorig.y) / raydir.y;

                hitpoint = rayorig + raydir * t;
                if (hitpoint.x >= min.x && hitpoint.x <= max.x &&
                    hitpoint.z >= min.z && hitpoint.z <= max.z &&
                    (!hit || t < lowt))
                {
                    hit = true;
                    lowt = t;
                }
            }

            if (rayorig.z <= min.z && raydir.z > 0)
            {
                t = (min.z - rayorig.z) / raydir.z;

                hitpoint = rayorig + raydir * t;
                if (hitpoint.x >= min.x && hitpoint.x <= max.x &&
                    hitpoint.y >= min.y && hitpoint.y <= max.y &&
                    (!hit || t < lowt))
                {
                    hit = true;
                    lowt = t;
                }
            }

            if (rayorig.z >= max.z && raydir.z < 0)
            {
                t = (max.z - rayorig.z) / raydir.z;

                hitpoint = rayorig + raydir * t;
                if (hitpoint.x >= min.x && hitpoint.x <= max.x &&
                    hitpoint.y >= min.y && hitpoint.y <= max.y &&
                    (!hit || t < lowt))
                {
                    hit = true;
                    lowt = t;
                }
            }

            return new KeyValuePair<bool, float>(hit, (float)lowt);
        }

        static public float DegreesToRadians(float degrees)
        {
            return degrees * fDeg2Rad;
        }

        static public float RadiansToDegrees(float radians)
        {
            return radians * fRad2Deg;
        }

        static public void setAngleUnit(AngleUnit unit)
        {
            msAngleUnit = unit;
        }

        static public AngleUnit getAngleUnit()
        {
            return msAngleUnit;
        }

        static public float AngleUnitsToRadians(float angleunits)
        {
            if (msAngleUnit == AngleUnit.AU_DEGREE)
                return angleunits * fDeg2Rad;
            else
                return angleunits;
        }

        static public float RadiansToAngleUnits(float radians)
        {
            if (msAngleUnit == AngleUnit.AU_DEGREE)
                return radians * fRad2Deg;
            else
                return radians;
        }

        static public float AngleUnitsToDegrees(float angleunits)
        {
            if (msAngleUnit == AngleUnit.AU_RADIAN)
                return angleunits * fRad2Deg;
            else
                return angleunits;
        }

        static public float DegreesToAngleUnits(float degrees)
        {
            if (msAngleUnit == AngleUnit.AU_RADIAN)
                return degrees * fDeg2Rad;
            else
                return degrees;
        }

        static public float Log2(float fValue)
        {
            return (float)(Mathf.Log(fValue) / LOG2);
        }

        static public float TILE_WIDTH = 2.5f;
        static public float TILE_HEIGHT = 2.5f;
        static public float MAP_WIDTH = 256f;
        static public float MAP_HEIGHT = 256f;

        static public MPointF convOrthoTile2IsoPt(MPointF orthoPt)
        {
            float xOffset = calcXOffset(new MPointF(0, MAP_HEIGHT));
            MPointF isoPt = new MPointF(0, 0);
            // orthoPt.x += xOffset; 先变换，然后再移动，先移动后变换是错误的
            isoPt.x = (orthoPt.x - orthoPt.y) * TILE_WIDTH / 2;
            isoPt.y = (orthoPt.x + orthoPt.y) * TILE_HEIGHT / 2;
            //isoPt.x += xOffset;
            return isoPt;
        }

        static public MPointF convIsoPt2OrthoTile(MPointF isoPt)
        {
            MPointF orthoPt = new MPointF(0, 0);
            orthoPt.x = isoPt.x / TILE_WIDTH + isoPt.y / TILE_HEIGHT;
            orthoPt.y = isoPt.y / TILE_HEIGHT - isoPt.x / TILE_WIDTH;
            return orthoPt;
        }

        static public float calcXOffset(MPointF orthoPt)
        {
            MPointF isoPt = new MPointF(0, 0);
            isoPt.x = (orthoPt.x - orthoPt.y) * TILE_WIDTH / 2;
            isoPt.y = (orthoPt.x + orthoPt.y) * TILE_HEIGHT / 2;
            return -isoPt.x;
        }

        static public MPointF convOrthoPt2IsoPt(MPointF orthoPt)
        {
            MPointF isoPt = new MPointF(0, 0);
            isoPt.x = 0.7071f * orthoPt.x - 0.7071f * orthoPt.y;
            isoPt.y = 0.7071f * orthoPt.x + 0.3535f * orthoPt.y;
            return isoPt;
        }

        static public MPointF convIsoPt2OrthoPt(MPointF isoPt)
        {
            MPointF orthoPt = new MPointF(0, 0);
            orthoPt.x = 0.7071f * isoPt.x + 0.7071f * isoPt.y;
            orthoPt.y = -0.7071f * isoPt.y + 1.4142f * isoPt.x;
            return orthoPt;
        }

        static public float floor(float f)
        {
            return Mathf.Floor(f);
        }

        static public int floorToInt(float f)
        {
            return Mathf.FloorToInt(f);
        }

        static public bool isEqualVec3(UnityEngine.Vector3 a, UnityEngine.Vector3 b)
        {
            if (UtilMath.Abs(a.x - b.x) > UtilMath.EPSILON ||
                UtilMath.Abs(a.y - b.y) > UtilMath.EPSILON ||
                UtilMath.Abs(a.z - b.z) > UtilMath.EPSILON)
            {
                return false;
            }

            return true;
        }

        static public bool isEqualQuat(UnityEngine.Quaternion a, UnityEngine.Quaternion b)
        {
            if (UtilMath.Abs(a.x - b.x) > UtilMath.EPSILON ||
                UtilMath.Abs(a.y - b.y) > UtilMath.EPSILON ||
                UtilMath.Abs(a.z - b.z) > UtilMath.EPSILON ||
                UtilMath.Abs(a.w - b.w) > UtilMath.EPSILON)
            {
                return false;
            }

            return true;
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            return UnityEngine.Vector3.Distance(a, b);
        }

        public static float squaredDistance(Vector3 a, Vector3 b)
        {
            float len = 0;

            Vector3 distVec = a - b;
            len = distVec.sqrMagnitude;

            return len;
        }

        // 获取单位前向向量
        public Vector3 getNormalForwardVector(Transform transform)
        {
            Vector3 dir = transform.localRotation * Vector3.forward;
            dir.Normalize();
            return dir;
        }

        /**
         * @param curOrient 当前方向
         * @param lookAt 观察点方向
         * @ret 返回旋转到观察点向量的四元数，这个是两个方向向量的夹角，不是点之间的夹角
         */
        public static UnityEngine.Quaternion getRotateByLookatPoint(UnityEngine.Quaternion curOrient, UnityEngine.Vector3 lookAt)
        {
            UnityEngine.Quaternion retQuat = curOrient;
            retQuat.SetLookRotation(lookAt);
            return retQuat;
        }

        /**
         * @param startPoint 开始点
         * @param destPoint 目标点
         * @ret 返回两个点之间的旋转的四元数，这个是两个点之间的夹角
         */
        public static UnityEngine.Quaternion getRotateByStartAndEndPoint(UnityEngine.Vector3 startPoint, UnityEngine.Vector3 destPoint)
        {
            UnityEngine.Quaternion retQuat;
            retQuat = UnityEngine.Quaternion.LookRotation(destPoint - startPoint);
            return retQuat;
        }

        /**
         * @param startOrient 开始方向
         * @param destOrient 目标方向
         * @ret 返回两个向量之间的旋转的四元数，这个是两个向量之间的夹角
         */
        public static UnityEngine.Quaternion getRotateByStartAndEndOrient(UnityEngine.Vector3 startOrient, UnityEngine.Vector3 destOrient)
        {
            UnityEngine.Quaternion retQuat;
            retQuat = UnityEngine.Quaternion.FromToRotation(startOrient, destOrient);

            return retQuat;
        }

        /**
         * @param forward 转向的方向
         * @ret 转向的四元数
         */
        public static UnityEngine.Quaternion getRotateByOrient(UnityEngine.Vector3 forward)
        {
            UnityEngine.Quaternion retQuat;
            retQuat = UnityEngine.Quaternion.LookRotation(forward);

            return retQuat;
        }

        public static bool isInvalidNum(float value)
        {
            return float.IsInfinity(value) ||
                   float.IsNaN(value) ||
                   float.IsNegativeInfinity(value) ||
                   float.IsPositiveInfinity(value);
        }

        public static int IntervalComparison(float x, float lowerBound, float upperBound)
        {
            if (x < lowerBound) return -1;
            if (x > upperBound) return +1;

            return 0;
        }

        /**
         * @brief 判断 A 是否在 C 的后面
          */
        public static bool isABehindC(UnityEngine.Vector3 aPoint, UnityEngine.Vector3 bPoint, UnityEngine.Vector3 cPoint)
        {
            UnityEngine.Vector3 ac = cPoint - aPoint;
            UnityEngine.Vector3 bc = cPoint - bPoint;

            if(UnityEngine.Vector3.Dot(ac, bc) > 0)
            {
                return true;
            }

            return false;
        }

        /**
         * @brief 判断碰撞对象是在前向还是后面，如果在球体的前半部分，与 forward 的关系是小于 90 ，否则大于 90
         * @param direct 运动方向
         * @param collision 碰撞信息
         */
        public static bool isBehindCollidePoint(UnityEngine.Vector3 pos, UnityEngine.Vector3 forward, Collision collision)
        {
            UnityEngine.Vector3 normal = collision.contacts[0].normal;

            if (UnityEngine.Vector3.Dot(normal, forward) < 0)
            {
                return true;
            }

            return false;
        }

        //由半径求质量
        public static float getMassByRadius(float radius)
        {
            return Mathf.Pow(radius, Ctx.mInstance.mSnowBallCfg.mRealMassFactor);
        }

        //由质量求半径
        public static float getRadiusByMass(float mass)
        {
            return Mathf.Pow(mass, 1 / Ctx.mInstance.mSnowBallCfg.mRealMassFactor);
        }

        //吞食后的新半径
        public static float getNewRadiusByRadius(float radius1, float radius2)
        {
            return getRadiusByMass(getMassByRadius(radius1) + getMassByRadius(radius2));
        }

        // 吞雪块后的半径是这个公式，这个特殊
        public static float getEatSnowNewRadiusByRadius(float radius)
        {
            return radius * (1 + 1 / (10 + Mathf.Pow(radius, Ctx.mInstance.mSnowBallCfg.mA)));
        }

        // 生成一个唯一 id
        public static ulong makeUniqueId(uint aId, uint bId)
        {
            ulong ret = 0;
            ret = ((aId << 31) | bId);
            return ret;
        }

        public static bool getSingleId(ulong uniqueId, ref uint aId, ref uint bId)
        {
            aId = (uint)(uniqueId >> 32);
            bId = (uint)(uniqueId & 0xFFFFFFFF00000000);
            return true;
        }
    }
}