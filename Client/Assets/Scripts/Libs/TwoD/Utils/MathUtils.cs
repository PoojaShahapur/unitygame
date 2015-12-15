namespace SDK.Lib
{
	public class MathUtils
	{
		public static float getAngle(float x1, float y1, float x2, float y2, float dist = 0)
		{
            float ret = UnityEngine.Mathf.Atan2(y2 - y1, x2 - x1);
			if (ret < 0)
				ret += 2 * UnityEngine.Mathf.PI;
			return ret * 180 / UnityEngine.Mathf.PI;
		}

		public static float distance(float x1, float y1, float x2, float y2)
		{
            float dx = x1 - x2;
            float dy = y2 - y1;
			return UnityEngine.Mathf.Sqrt(dx * dx + dy * dy);
		}
		
		// 距离的平方，不开平方  
		public static float distanceSquare(float x1, float y1, float x2, float y2)
		{
            float dx = x1 - x2;
            float dy = y2 - y1;
			return (dx * dx + dy * dy);
		}

		public static float distance3d(float x1, float y1, float z1, float x2, float y2, float z2)
		{
            float dx = x1 - x2;
            float dy = y2 - y1;
            float dz = z2 - z1;
			return UnityEngine.Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
		}

		/*
		 *沿指定方向(direction)移动distance后，计算其位置偏移量(返回值)
		 *其公式是：result.x = (direction.x/|direction|)*dist
		 * 			result.y = (direction.y/|direction|)*dist
		 */
		public static PointF displacementVector(PointF direction, float dist)
		{
			
			if (direction == null)
			{
				return null;
			}
            PointF result = new PointF(0, 0);
			float scale = dist / MathUtils.distance(direction.x, direction.y, 0, 0);
			
			result.x = scale * direction.x;
			result.y = scale * direction.y;
			return result;
		}
	}
}