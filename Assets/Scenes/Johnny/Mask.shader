Shader "Custom/Mask"
{

  SubShader
  {
	 Tags {"Queue" = "Transparent+2"}	 

  Pass
     {
		 Blend Zero One 
     }
  }

}
