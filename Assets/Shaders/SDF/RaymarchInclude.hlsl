#ifndef RAYMARCH_INCLUDED
#define RAYMARCH_INCLUDED

#include "Assets/Shaders/SDF/RaymarchUtils.hlsl"

#define MAX_STEPS 100
#define MAX_DIST 100
#define SURF_DIST 1e-3


float4 GetDist(float3 p) {
    float4 d;
    d.xyz = 1;
    d.w = p.y;
    return d;
}

float3 GetNormal(float3 p) {
    float2 e = float2(1e-2, 0);
    //float3 n = GetDist(p).w - float3(GetDist(p + e.xyy).w, GetDist(p + e.yxy).w, GetDist(p + e.yyx).w);
    float3 n = float3(0,1,0);

    return normalize(n);
}

void Raymarch_half(half3 ro, half3 rd, half3 rdMid, half depth, half minDepth, out half3 p, out half3 n, out half3 c, out half a) {
    half dO = 0;
    half4 dS;
    for (int i = 0; i < MAX_STEPS; i++) {
        if (dO > MAX_DIST) {
            break;
        }
        half3 p = ro + dO * rd;
        dS = GetDist(p);
        if (dS.w < SURF_DIST) {
            c = dS.xyz;
            break;
        }
        dO += dS.w;
    }
    //if (dO < MAX_DIST && dO > minDist) {
    if (dO < MAX_DIST && dO * dot(rd, rdMid) < depth || minDepth < depth) {
        p.xyz = ro + rd * dO;
        a = 1;
        n = GetNormal(p);
        c = dS.xyz;
    } else {
        p = half3(0,0,0);
        a = 0;
        c = half3(1,1,1);
        n = half3(0,0,0);
    }
    //return dO;
}


#endif