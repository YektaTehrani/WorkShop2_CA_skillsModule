using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour {

	// VARIABLES
    //state
	private int state = 0;
    //next state
    private int futureState = 0;
    //age
    private int age = 0;

    public int maxage = 7;
    //density3dMO
    private int density3dMO = 0;
    //density3dVN
    private int density3dVN = 0;
    //density3dYT
    private int density3dYT = 0;
    //densityedNR
    private int density3dNR = 0;
    //density3dMT
    private int density3dMT = 0;
    //material property block for setting material properties with renderer
    private MaterialPropertyBlock props;
    //the mesh renderer
    private new MeshRenderer renderer;
    //var stores my 3d address
	public Vector3 address;

    //The Mesh Filter takes a mesh from your assets and passes it to the Mesh Renderer for rendering on the screen
    //One Voxel can contain different meshes which are the representation of different types of voxels
    public MeshFilter type0mesh, type1Mesh, type2Mesh, type3Mesh;

    //variable to store a type for this voxel
	int type;

    //von neumann neighbors
    private Voxel[] neighbors3dVN = new Voxel[6];

    //moore's neighbors
    private Voxel[] neighbors3dMO = new Voxel[26];
    //Yekta's neighbors
    private Voxel[] neighbors3dYT;

    //Matt's neighbors
    private Voxel[] neighbors3dMT;

    //Noura's neighbors
    private Voxel[] neighbors3dNR;

    private Voxel voxelAbove;
    private Voxel voxelBelow;
    private Voxel voxelRight;
    private Voxel voxelLeft;
    private Voxel voxelFront;
    private Voxel voxelBack;
    private Voxel vox1;
    private Voxel vox2;
    private Voxel vox3;
    private Voxel vox4;
    private Voxel vox5;
    private Voxel vox6;
    private Voxel vox7;
    private Voxel vox8;
    private Voxel vox9;


    //Yekta's voxels
    public Voxel voxel_1;
    public Voxel voxel_2;
    public Voxel voxel_3;
    public Voxel voxel_4;
    public Voxel voxel_5;
    public Voxel voxel_6;
    public Voxel voxel_7;
    public Voxel voxel_8;

    //Noura's voxels
    public Voxel lilac;
    public Voxel lily;
    public Voxel tulip;

    //Matt's voxels
    public Voxel Wuw;
    public Voxel Lily2;


    //colors
    Color savedColor;
    public int maxAge;


    // FUNCTIONS


    public void SaveColor(int _r, int _g, int _b)
    {
        savedColor = new Color(-_r, _g, _b);
    }

    public void SetupVoxel(int i, int j, int k, int _type)
    {

        savedColor = new Color(1, 1, 1);
        neighbors3dMO = new Voxel[26];
        neighbors3dVN = new Voxel[6];
        neighbors3dYT = new Voxel[8];
        neighbors3dNR = new Voxel[3];
        neighbors3dMT = new Voxel[2];
        //set reference to time end 
        props = new MaterialPropertyBlock();
        renderer = gameObject.GetComponent<MeshRenderer>();
        //initially set to false
        renderer.enabled = false;
        //set my address as a vector
		address = new Vector3 (i,j,k);

        //gets the type of this voxel and sets the mesh filter by type - allows us to preload
        //different meshes and render a different mesh for different voxels based on the type
		type = _type;
		switch (type) {
		case 1:
			MeshFilter setMesh = gameObject.GetComponent<MeshFilter> ();
			setMesh = type1Mesh;
			break;
		case 2:
			MeshFilter setMesh2 = gameObject.GetComponent<MeshFilter> ();
			setMesh2 = type2Mesh;
			break;
		case 3:
			MeshFilter setMesh3 = gameObject.GetComponent<MeshFilter> ();
			setMesh3 = type3Mesh;
			break;	
		default:
			MeshFilter setMeshDefault = gameObject.GetComponent<MeshFilter> ();
			setMeshDefault = type3Mesh;
			break;
		}
    }

	// Update function
	public void UpdateVoxel () {
		// Set the future state
		state = futureState;
        // If voxel is alive update age
        if (state == 1)
        {
            age++;
        }
        // If voxel is death disable the game object mesh renderer and set age to zero
        if (state == 0)
        {
            age = 0;
        }
    }


    /// <summary>
    /// Setters and Getters - Allow us to access and set private variables
    /// </summary>
    /// <param name="_state"></param>
	// Set the state of the voxel
	public void SetState(int _state){
		state = _state;
	}

	// Set the future state of the voxel
	public void SetFutureState(int _futureState){
		futureState = _futureState;
	}

    // Get the age of the voxel
	public void SetAge(int _age){
		age = _age;
	}

	// Get the state of the voxel
	public int GetState(){
		return state;
	}

	// Get the age of the voxel
	public int GetAge(){
		return age;
	}

    //Set 3d Moores Neighborhood Density 
    public void setDensity3dMO(int _density3dMO)
    {
        density3dMO = _density3dMO;
    }
    //Get 3d Moores Neighborhood Density 
    public int getDensity3dMO()
    {
        return density3dMO;
    }

    //Set 3d Von Neumann Neighborhood Density 
    public void setDensity3dVN(int _density3dVN)
    {
        density3dVN = _density3dVN;
    }
    //Get 3d Von Neumann Neighborhood Density 
    public int getDensity3dVN()
    {
        return density3dVN;
    }
    //Set 3d Yekta Neighborhood Denisty
    public void setDensity3dYT(int _density3dYT)
    {
        density3dYT = _density3dYT;
    }

    //Get 3d Yekta Neighborhood Denisty

    public int getDensity3dYT()
    {
        return density3dYT;
    }

    //Set 3d Noura's Neighborhood Denisty
    public void setDensity3dNR(int _density3dNR)
    {
        density3dNR = _density3dNR;
    }

    //Get 3d Noura's Neighborhood Denisty

    public int getDensity3dNR()
    {
        return density3dNR;
    }

    //Set 3d Matt's Neighborhood Denisty
    public void setDensity3dMT(int _density3dMT)
    {
        density3dMT = _density3dMT;
    }

    //Get 3d Matt's's Neighborhood Denisty

    public int getDensity3dMT()
    {
        return density3dMT;
    }



    /// <summary>
    /// VOXEL NEIGHBORHOOD GETTERS/SETTERS
    /// </summary>
    /// 

    //MOORES NEIGHBORS (26 PER VOXEL)
    public void setNeighbors3dMO(Voxel[] _setNeighbors3dMO)
    {
        neighbors3dMO = _setNeighbors3dMO;
    }

    public Voxel[] getNeighbors3dMO()
    {
        return neighbors3dMO;
    }

    //VON NEUMANN NEIGHBORS (6 PER VOXEL)
    public void setNeighbors3dVN(Voxel[] _setNeighbors3dVN)
    {
        neighbors3dVN = _setNeighbors3dVN;
    }

    public Voxel[] getNeighbors3dVN()
    {
        return neighbors3dVN;
    }
    //
    //
    //Yekta Neighbors (8 per Voxel)
    public void setNeighbors3dYT(Voxel[] _setNeighbors3dYT)
    {
        neighbors3dYT = _setNeighbors3dYT;
    }

    public Voxel[] getNeighbors3dYT()
    {
        return neighbors3dYT;
    }
    //
    //
    //
    //Noura's Neighbors (3 per Voxel)
    public void setNeighbors3dNR(Voxel[] _setNeighbors3dNR)
    {
        neighbors3dNR = _setNeighbors3dNR;
    }

    public Voxel[] getNeighbors3dNR()
    {
        return neighbors3dNR;
    }
    //
    //
    //
    //Matt's Neighbors (2 per Voxel)
    public void setNeighbors3dMT(Voxel[] _setNeighbors3dMT)
    {
        neighbors3dMT = _setNeighbors3dMT;
    }

    public Voxel[] getNeighbors3dMT()
    {
        return neighbors3dMT;
    }
    //
    //

    //voxel above this
    public void setVoxelAbove(Voxel _voxelAbove)
    {
        voxelAbove = _voxelAbove;
    }

    public Voxel getVoxelAbove()
    {
        return voxelAbove;
    }

    //voxel below this
    public void setVoxelBelow(Voxel _voxelBelow)
    {
        voxelBelow = _voxelBelow;
    }

    public Voxel getVoxelBelow()
    {
        return voxelBelow;
    }

    //voxel right of this
    public void setVoxelRight(Voxel _voxelRight)
    {
        voxelRight = _voxelRight;
    }

    public Voxel getVoxelRight()
    {
        return voxelRight;
    }

    //voxel left of this
    public void setVoxelLeft(Voxel _voxelLeft)
    {
        voxelLeft = _voxelLeft;
    }

    public Voxel getVoxelLeft()
    {
        return voxelLeft;
    }

    //voxel in front of this
    public void setVoxelFront(Voxel _voxelFront)
    {
        voxelFront = _voxelFront;
    }

    public Voxel getVoxelFront()
    {
        return voxelFront;
    }

    //voxel in back of this
    public void setVoxelBack(Voxel _voxelBack)
    {
        voxelBack = _voxelBack;
    }

    public Voxel getVoxelBack()
    {
        return voxelBack;
    }

    public void setVox1(Voxel _vox1)
    {
        vox1 = _vox1;
    }

    public Voxel getVox1()
    {
        return vox1;
    }
    public void setVox2(Voxel _vox2)
    {
        vox2 = _vox2;
    }

    public Voxel getVox2()
    {
        return vox2;
    }
    public void setVox3(Voxel _vox3)
    {
        vox3 = _vox3;
    }

    public Voxel getVox3()
    {
        return vox3;
    }
    public void setVox4(Voxel _vox4)
    {
        vox4 = _vox4;
    }

    public Voxel getVox4()
    {
        return vox4;
    }
    public void setVox5(Voxel _vox5)
    {
        vox5 = _vox5;
    }

    public Voxel getVox5()
    {
        return vox5;
    }
    public void setVox6(Voxel _vox6)
    {
        vox6 = _vox6;
    }

    public Voxel getVox6()
    {
        return vox6;
    }

    public void setVox7(Voxel _vox7)
    {
        vox7 = _vox7;
    }

    public Voxel getVox7()
    {
        return vox7;
    }
    public void setVox8(Voxel _vox8)
    {
        vox8 = _vox8;
    }

    public Voxel getVox8()
    {
        return vox8;
    }
    public void setVox9(Voxel _vox9)
    {
        vox9 = _vox9;
    }

    public Voxel getVox9()
    {
        return vox9;
    }


    // Update the voxel display
    public void VoxelDisplay()
    {
        if (state == 1)
        {
            // Set Color
            Color col = new Color(1, 1, 1, 1);
            props.SetColor("_Color", savedColor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }

        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    public void VoxelDisplay(int _r, int _g, int _b)
    {
        if (state == 1)
        {
            // Set Color
            Color col = new Color(_r, _g, _b, 1);
            props.SetColor("_Color", col);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }

        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// Create Color Gradient Between 2 Colors by Age
    /// </summary>
    /// <param name="_maxAge"></param>
    public void VoxelDisplayAge(int _maxAge)
    {
        if (state == 1)
        {
            // Remap the age value relative to maxage to range of 0,1
            float mappedvalue = Remap(age, 0, _maxAge, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 1, 1);
            Color color2 = new Color(0.6f, 1, 0.6f);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }



    /// <summary>
    /// Create Color Gradient Between 2 Colors by Density
    /// </summary>
    /// <param name="_maxdensity3dMO"></param>
    public void VoxelDisplayDensity3dMO(int _maxdensity3dMO)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dMO, 0, _maxdensity3dMO, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 1, 1, 1);
            Color color2 = new Color(0.3f, 1, 0.3f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// Create Color Gradient Between 2 Colors by Density
    /// </summary>
    /// <param name="_maxdensity3dMO"></param>
    public void VoxelDisplayDensity3dVN(int _maxdensity3dVN)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dMO, 0, _maxdensity3dVN, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 1, 1, 1);
            Color color2 = new Color(0, 1, 1, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }
    //YT Voxel density YT
    public void VoxelDisplayDensityedYT(int _maxdensity3dYT)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dYT, 0, _maxdensity3dYT, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 1, 1, 1);
            Color color2 = new Color(1f, 0.92f, 0.016f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    //NR Voxel density NR
    public void VoxelDisplayDensityedNR(int _maxdensity3dNR)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dNR, 0, _maxdensity3dNR, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 1, 1, 1);
            Color color2 = new Color(1f, 0f, 0f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    //NR Voxel density MT
    public void VoxelDisplayDensityedMT(int _maxdensity3dMT)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(density3dMT, 0, _maxdensity3dMT, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 1, 1, 1);
            Color color2 = new Color(1f, 0f, 1f, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }
    public void VoxelDisplayLayerDensity(float _layerDensity, float _minlayerdensity, float _maxlayerdensity)
    {
        if (state == 1)
        {
            // Remap the density value relative to maxdensity to range of 0,1
            float mappedvalue = Remap(_layerDensity, 0, _maxlayerdensity, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(1, 1, 1, 1);
            Color color2 = new Color(0, 0, 1, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);
            props.SetColor("_Color", mappedcolor);
            // Updated the mesh renderer color
            renderer.enabled = true;
            renderer.SetPropertyBlock(props);
        }
        if (state == 0)
        {
            renderer.enabled = false;
        }
    }

    // Remap numbers - used here for getting a gradient of color across a range
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
