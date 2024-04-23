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

void Raymarch_half(half3 ro, half3 rd, half3 rdMid, half depth, half minDepth, out half3 p, out half3 n, out half3 c, out half a, out half d) {
    half dO = 0;
    half realDepth = depth / dot(rd, rdMid);
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
    /*
    //if (dO < MAX_DIST && dO > minDist) {
    if (dO < MAX_DIST && (dO < realDepth || minDepth < realDepth)) {
        // camera seeing ground plane
        p.xyz = ro + rd * dO;
        //a = saturate((depth / dot(rd, rdMid) - minDepth) * 10);
        a = 1;
        n = GetNormal(p);
        c = dS.xyz;
        d = dO * dot(rd, rdMid);

    } else {
        // camera seeing object
        p = half3(0,0,0);
        a = 0;
        c = half3(1,1,1);
        n = half3(0,0,0);
        d = depth;
    }
    */

    if (dO > realDepth && minDepth > realDepth) {
        // camera seeing object
        p = half3(0,0,0);
        a = 0;
        c = half3(1,1,1);
        n = half3(0,0,0);
        d = depth;
    } else if (dO < MAX_DIST) {
        // camera seeing SDF
        p.xyz = ro + rd * dO;
        //a = saturate((depth / dot(rd, rdMid) - minDepth) * 10);
        a = 1;
        n = GetNormal(p);
        c = dS.xyz;
        d = dO * dot(rd, rdMid);
    } else {
        // camera seeing void
        p = ro + rd * MAX_DIST;
        a = 1;
        c = -rd;
        n = -rd;
        d = depth;
    }

}


#endif