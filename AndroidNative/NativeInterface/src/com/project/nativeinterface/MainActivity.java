package com.project.nativeinterface;

import android.os.Bundle;
import com.unity3d.player.UnityPlayerActivity;

public class MainActivity extends UnityPlayerActivity 
{
    @Override
    protected void onCreate(Bundle savedInstanceState) 
    {
        super.onCreate(savedInstanceState);
    }
    
    public int Max(int a, int b)
    {
    	if(a < b)
    	{
    		return a;
    	}
    	
    	return b;
    }
}
