struct SkyColorSet
{
    float4 sky;
    float4 shadow;
    float4 overlay;
    float saturation;
};

struct GlowSource
{
    uint index;
    bool active;
    float radius;
    float overlightRadius;
    float4 color;
};

struct LightBlocker
{
    int2 Min;
    int2 Max;
};

struct Blocker
{
    uint2 BL;
    uint2 BR;
    uint2 TL;
    uint2 TR;
};

int2 IndexToPos(int index, int2 size)
{
    return int2(index % size.x, index / size.y);
}
