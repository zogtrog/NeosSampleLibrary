 a= new Array;a[0]='toaster';
 for(i=0;i<100;i++)
  { 
	if(inputReady()) 
	{
		var c=getInput();
		var b = JSON.parse(c); 
		a[0] = b.nv;
  	}
 sleep(1000); 
 var PInner = {"inner_value":i*i};
 //Complex output
 var POut = {"index":a[0] + ' ' + i,inner:PInner};
result(POut);
}