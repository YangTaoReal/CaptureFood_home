Shader "Custom/Stream" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MainTint ("Diffuse Tint", Color) = (1,1,1,1)
        _ScrollXSpeed ("X Scroll Speed", Range(0,10)) = 2
        _ScrollYSpeed ("Y Scroll Speed", Range(0,10)) = 2
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
 
        // 将类型转换到CG中的可用类型
        sampler2D _MainTex;
        fixed4 _MainTint;
        fixed _ScrollXSpeed;
        fixed _ScrollYSpeed;
 
        // 读取模型数据的结构体，名字有些迷惑人
        struct Input {
            float2 uv_MainTex;
        };
 
        // inout类型是一个输出数据的类型，顶点着色器处理好数据之后传递给片段着色器
        // 就是通过inout修饰的参数来传递的，也就是说inout修饰的类型是提供给片段着色器进一步渲染的     
        void surf (Input IN, inout SurfaceOutputStandard o) {
        
            // 先将UV坐标保存起来（这个值的类型是float2或者fixed2都可以）
            float2 scrolledUV = IN.uv_MainTex;
 
            // 可以根据自己的需求来决定滚动方向，此处为xy两个方向偏移
            // _Time：是基于Unity的系统内置的时间变量
            fixed xScrollValue = _ScrollXSpeed * _Time;
            fixed yScrollValue = _ScrollYSpeed * _Time;
 
            // fixed2（x，y）：这个就不用过多解释了，将xy合并成一个二元的数据类型
            scrolledUV += fixed2(xScrollValue,yScrollValue);
 
            // tex2D（sampler2D，fixed2）官方定义叫：二维纹理查询
            // 给出一张图和纹理坐标计算出颜色
            half4 c = tex2D (_MainTex, scrolledUV)* _MainTint;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}