vec3 lerp(vec3 v0, vec3 v1, float t){
	return v0 + t * (v1 - v0);
}

float inv_lerp(float v0, float v1, float v){
 return (v - v0) / (v1 - v0);
}