using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Environment : MonoBehaviour
{
    public int voxage;

    public int maxvoxage;

    int totalPopulation;

	// VARIABLES
    public Text popText;

    public Text allPopText;
    
    private int popCount;

	// Texture to be used as start of CA input
	public Texture2D seedImage;
    
	// Number of frames to run which is also the height of the CA
	public int timeEnd = 2;
	int currentFrame = 0;

    //variables for size of the 3d grid
	int width;
	int length;
	int height;

    // Array for storing voxels
    GameObject[,,] voxelGrid;

	// Reference to the voxel we are using
	public GameObject voxelPrefab;

    public GameObject aboveVox;
	// Spacing between voxels
	float spacing = 1.0f;

    //Layer Densities
    int totalAliveCells = 0;
    float layerdensity = 0;
    float[] layerDensities; // array
    private float maxlayerdensity=0;
    private float minlayerdensity = 100000000;


    //Max Age
    int maxAge = 0;

    //Max Densities
    int maxDensity3dMO = 0;
    int maxDensity3dVN = 0;
    int maxDensity3dYT = 0;
    int maxDensity3dNR = 0;
    int maxDensity3dMT = 0;

    // Setup Different Game of Life Rules
    GOLRule deathrule = new GOLRule();
    GOLRule rule1 = new GOLRule();
    GOLRule rule2 = new GOLRule();
    GOLRule rule3 = new GOLRule();
    GOLRule densityrules = new GOLRule();

    //boolean switches
    //toggles pausing the game
    bool pause = false;

    private int vizmode = 0;

	// FUNCTIONS

	// Use this for initialization
	void Awake () {
		// Read the image width and height
		width = seedImage.width;
		length = seedImage.height;
		height = timeEnd;

        //Setup GOL Rules
        //rule1.setupRule(2, 1, 3, 2);
        //rule2.setupRule(1, 2, 4, 5);
        //deathrule.setupRule(0, 0, 0, 0);
        ///
        /// 
        //rule1.setupRule(1, 1, 2, 6);
        //rule2.setupRule(1, 1, 4, 5);
        //deathrule.setupRule(0, 0, 0, 0);
        ///
        /// 
        //rule1.setupRule(2, 1, 2, 1);
        //rule2.setupRule(1, 2, 1, 5);
        //deathrule.setupRule(0, 0, 0, 0);
        ///
        /// 
	    //rule1.setupRule(1, 1, 2, 1);////////////////////////////////// stepping behaviour
        //rule2.setupRule(1, 1, 1, 1);//////////////////////////////
        //rule3.setupRule(1,2,1,2);
        //densityrules.setupRule(1,1,1,1);
        //deathrule.setupRule(0, 0, 0, 0);//////////////////
        ///
		rule1.setupRule(1, 1, 3, 1);////////////////////////////////// stepping behaviour
		rule2.setupRule(2, 5, 7, 1);//////////////////////////////
		rule3.setupRule(1,2,0,1);
		densityrules.setupRule(1,0,0,1);
		deathrule.setupRule(0, 0, 0, 0);//////////////////
		///
        /// 
        //rule1.setupRule(8, 4, 3, 2);
        //rule2.setupRule(4, 2, 1, 5);
        //   rule3.setupRule(4,2,3,2);
        //deathrule.setupRule(0, 0, 0, 0);
        ///
        /// 
        /// 
        //rule1.setupRule(8, 4, 3, 2);
        //rule2.setupRule(4, 2, 1, 5);
        //deathrule.setupRule(0, 0, 0, 0);
        /// 
        /// 
        //rule1.setupRule(1, 3, 3, 4);
        //rule2.setupRule(1, 2, 4, 5);
        //deathrule.setupRule(0, 0, 0, 0);

        //Layer Densities
        layerDensities = new float[timeEnd];

        // Create a new CA grid
        CreateGrid ();
        SetupNeighbors3d();
	    popCount = 0;
	   
	    //popText.text = "Population: " + totalAliveCells.ToString();
	}
	
	// Update is called once per frame
	void Update () {

        // Calculate the CA state, save the new state, display the CA and increment time frame
        if (currentFrame < timeEnd - 1)
        {
            if (pause == false)
            {
                        // Calculate the future state of the voxels
            CalculateCA();
            // Update the voxels that are printing
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    GameObject currentVoxel = voxelGrid[i, j, 0];
                    currentVoxel.GetComponent<Voxel>().UpdateVoxel();
                }

            }
            // Save the CA state
                SaveCA();
                AllVoxCounter();
                SetCountText();
                SetCountText2();
                //Update 3d Densities
                updateDensities3d();
            // Increment the current frame count
                currentFrame++;
            }

            
        }
        else
        {
           // PrintAge();
        }

        // Display the voxels
	    // Display the printed voxels
	    for (int i = 0; i < width; i++)
	    {
	        for (int j = 0; j < length; j++)
	        {
	            for (int k = 1; k < height; k++)
	            {
	                if (vizmode == 0)
	                {
	                    voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplay();
	                }
	                if (vizmode == 1)
	                {
	                    voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayAge(maxAge);
	                }
	                if (vizmode == 2)
	                {
	                    voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensity3dMO(maxDensity3dMO);
	                }
	                if (vizmode == 3)
	                {
	                    voxelGrid[i, j, k].GetComponent<Voxel>()
	                        .VoxelDisplayLayerDensity(layerDensities[k], minlayerdensity, maxlayerdensity);
	                }
                    //voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplay();
                    //voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayAge(maxAge);
                    //voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensity3dMO(maxDensity3dMO);
                    //voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensity3dVN(maxDensity3dVN);
                    voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensityedYT(maxDensity3dYT);
                    //voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensityedNR(maxDensity3dNR);
                    //voxelGrid[i, j, k].GetComponent<Voxel>().VoxelDisplayDensityedMT(maxDensity3dMT);
                }
            }

	    }
        
	       

			/////////////////////////////
			if (Input.GetKeyDown(KeyCode.E))
			{
				ExportPrepare();
			}
			///////////////////////////
		

    
       

        int voxel101010Density = voxelGrid[10, 10, 10].GetComponent<Voxel>().getDensity3dVN();
	    print("The voxel 10,10,10 density is:" + voxel101010Density.ToString());

	    KeyPressMethod();

    }

    public void KeyPressMethod()
    {

        // Spin the CA if spacebar is pressed (be careful, GPU instancing will be lost!)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<ModelDisplay>() == null)
            {
                gameObject.AddComponent<ModelDisplay>();
            }
            else
            {
                Destroy(gameObject.GetComponent<ModelDisplay>());
            }
        }


        //toggle pause with "p" key
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pause == false)
            {
                pause = true;
            }
            else
            {
                pause = false;
            }
        }

        //toggle pause with "p" key
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (vizmode <= 3)
            {
                vizmode++;
            }
            if (vizmode > 3)
            {
                vizmode = 0;
            }
        }
    }


    // Create grid function
    void CreateGrid(){
		// Allocate space in memory for the array
		voxelGrid = new GameObject[width, length, height];
		// Populate the array with voxels from a base image
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				for (int k = 0; k < height; k++) {
					// Create values for the transform of the new voxel
					Vector3    currentVoxelPos = new Vector3 (i*spacing,k*spacing,j*spacing);
					Quaternion currentVoxelRot = Quaternion.identity;
                    //create the game object of the voxel
					GameObject currentVoxelObj = Instantiate (voxelPrefab, currentVoxelPos, currentVoxelRot);
                    //run the setupVoxel() function inside the 'Voxel' component of the voxelPrefab
                    //this sets up the instance of Voxel class inside the Voxel game object
                    currentVoxelObj.GetComponent<Voxel>().SetupVoxel(i,j,k,1);

                    // Set the state of the voxels
                    if (k == 0) {						
						// Create a new state based on the input image
						int currentVoxelState = (int)seedImage.GetPixel (i, j).grayscale;
                        currentVoxelObj.GetComponent<Voxel> ().SetState (currentVoxelState);
                    } else {
                        // Set the state to death
                        currentVoxelObj.GetComponent<MeshRenderer>().enabled = false;
                        currentVoxelObj.GetComponent<Voxel> ().SetState (0);
                    }
					// Save the current voxel in the voxelGrid array
					voxelGrid[i,j,k] = currentVoxelObj;
                    // Attach the new voxel to the grid game object
                    currentVoxelObj.transform.parent = gameObject.transform;
				}
			}
		}
	}

	// Calculate CA function
  public void CalculateCA(){
		// Go over all the voxels stored in the voxels array
		for (int i = 1; i < width-1; i++) {
			for (int j = 1; j < length-1; j++) {
				GameObject currentVoxelObj = voxelGrid[i,j,0];
				int currentVoxelState = currentVoxelObj.GetComponent<Voxel> ().GetState ();
				int aliveNeighbours = 0;

				// Calculate how many alive neighbours are around the current voxel
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						GameObject currentNeigbour = voxelGrid [i + x, j + y,0];
						int currentNeigbourState = currentNeigbour.GetComponent<Voxel> ().GetState();
						aliveNeighbours += currentNeigbourState;
					}
				}



                aliveNeighbours -= currentVoxelState;

                //CHANGE RULE BASED ON CONDITIONS HERE:
                GOLRule currentRule = rule1;
                //CHANGE RULE BASED ON CONDITIONS HERE:
                //..........

            //   if (currentFrame > 10)
            //   {
          //       currentRule = rule2;

          //    }
       //         if (currentFrame > 20)
       //         {
       //             currentRule = rule1;
                   

       //         }
       //         if (currentFrame > 30)
       //         {
       //             currentRule = rule2;

       //         }
       //         if (currentFrame > 40)
       //         {
       //             currentRule = rule1;
       //         }
       //         if (currentFrame > 50)
       //         {
       //             currentRule = rule2;
       //         }
			    //if (currentFrame > 60)
			    //{
			    //    currentRule = rule1;
			    //}
			    /*
                if (currentVoxelObj.GetComponent<Voxel>().GetAge()>3)
                {
                    currentRule = deathrule;
                }

                if (layerdensity < 0.2)
                {
                    currentRule = rule2;
                }
                */
                //..........

                //get the instructions
                int inst0 = currentRule.getInstruction(0);
                int inst1 = currentRule.getInstruction(1);
                int inst2 = currentRule.getInstruction(2);
                int inst3 = currentRule.getInstruction(3);

			    Voxel wow;
			    Voxel wuw;
			    Voxel wew;  
			    Voxel rose;
			    Voxel lilac;
			    Voxel lily;
			    Voxel tulip;
			    Voxel orchid;
           
                wow = voxelGrid[i , j+1, 0].GetComponent<Voxel>();
                currentVoxelObj.GetComponent<Voxel>().setVox1(wow);

                wuw = voxelGrid[i+1, j + 1, 0].GetComponent<Voxel>();
                currentVoxelObj.GetComponent<Voxel>().setVox2(wuw);

			    wew = voxelGrid[i+1, j , 0].GetComponent<Voxel>();
                currentVoxelObj.GetComponent<Voxel>().setVox3(wew);

			    rose = voxelGrid[i - 1, j + 1, 0].GetComponent<Voxel>();
                currentVoxelObj.GetComponent<Voxel>().setVox4(rose);

			    lilac = voxelGrid[i - 1, j, 0].GetComponent<Voxel>();
                currentVoxelObj.GetComponent<Voxel>().setVox5(lilac);

			    lily = voxelGrid[i - 1, j - 1, 0].GetComponent<Voxel>();
                currentVoxelObj.GetComponent<Voxel>().setVox6(lily);

			    tulip = voxelGrid[i, j - 1, 0].GetComponent<Voxel>();
                currentVoxelObj.GetComponent<Voxel>().setVox7(tulip);

			    orchid = voxelGrid[i+1, j - 1, 0].GetComponent<Voxel>();
                currentVoxelObj.GetComponent<Voxel>().setVox8(orchid);

                // Rule Set 1: for voxels that are alive
                if (currentVoxelState == 1) {
					// If there are less than two neighbours I am going to die
					if (aliveNeighbours < inst0) {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState (0);
                      // orchid.SetFutureState(0);
					  
					    //activating color for different behaviours
					    //lily.SaveColor(1, 0, 0);
					    //orchid.SaveColor(0, 1, 0);
                        


                    }
                    if (orchid.getDensity3dVN()<=4)
                    {
                       orchid.SetFutureState(0);
                    }

                    if (lily.getDensity3dMO()>=1)
                    {
                        lily.SetFutureState(0);
                    }
                    //if (lilac.getDensity3dMO() <= 3)
                    //{
                    //    lilac.SetFutureState(1);
                    //}
                    //if (rose.getDensity3dMO() >= 2)
                    //{
                    //    lily.SetFutureState(1);
                    //}

                    // If there are two or three neighbours alive I am going to stay alive
                    if (aliveNeighbours >= inst0 && aliveNeighbours <= inst1)
                    {
                        //currentVoxelObj.GetComponent<Voxel> ().SetFutureState (1);
                       currentVoxelObj.GetComponent<Voxel>().SetFutureState(1);
                       // if (orchid.GetState() == 1)
                       // {
                           // if (orchid.GetAge()>=maxvoxage)
                           // {
                            //wuw.SetFutureState(1);
                            //    orchid.SetFutureState(1);
                           // }
                            lily.SetFutureState(1);
                           // if (lily.GetAge()>=maxvoxage)
                           // {
                          //      lily.SetFutureState(0);
                          //  }
                       // }
                      //  else
                       // {
                      //      lily.SetFutureState(0);
                      //  }
                      ///
                      /// 
                      /// 
                      /// 
                        lilac.SetFutureState(1);
                        rose.SetFutureState(1);
                        ///
                        /// 
                        /// 
                        /// 
                    }
                    //If there are more than three neighbours I am going to die
                    if (aliveNeighbours > inst2)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                        tulip.SetFutureState(1);
                    }   
                }




			    int wowage = wow.GetComponent<Voxel>().GetAge();
			    int wewage = wew.GetComponent<Voxel>().GetAge();
			    int wuwage = wuw.GetComponent<Voxel>().GetAge();
			    int roseage = rose.GetComponent<Voxel>().GetAge();
			    int lilacage = lilac.GetComponent<Voxel>().GetAge();
			    int lilyage = lily.GetComponent<Voxel>().GetAge();
			    int tulipage = tulip.GetComponent<Voxel>().GetAge();
			    int orchidage = orchid.GetComponent<Voxel>().GetAge();


                maxvoxage = currentVoxelObj.GetComponent<Voxel>().maxage;
                voxage    = currentVoxelObj.GetComponent<Voxel>().GetAge();




                if (voxage > maxvoxage)
                {

                    //if (aliveNeighbours <= 2)
                    //{
                    //    currentVoxelObj.GetComponent<Voxel>().SetFutureState(1);
                    //}
                    //if (aliveNeighbours == 4 || aliveNeighbours == 3)
                    //{
                    //    currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                    //}
                    currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);

                }
                if (wowage > maxvoxage)
                {
                    wow.SetFutureState(0);
                }
                if (wewage > maxvoxage)
                {
                    wew.SetFutureState(0);
                }
                if (wuwage > maxvoxage)
                {
                    wuw.SetFutureState(0);
                }
                if (roseage > maxvoxage)
                {
                    rose.SetFutureState(0);
                }
                if (lilacage > maxvoxage)
                {
                    lilac.SetFutureState(1);
                }
                if (lilyage > maxvoxage)
                {
                    lily.SetFutureState(0);
                }
                if (tulipage > maxvoxage)
                {
                    tulip.SetFutureState(0);
                }
                if (orchidage > maxvoxage)
                {
                    orchid.SetFutureState(0);
                }
                // Rule Set 2: for voxels that are death
                if (currentVoxelState == 0)
                {
                    //	// If there are exactly three alive neighbours I will become alive
                    if (aliveNeighbours >= inst2 && aliveNeighbours <= inst3)
                    {
                        currentVoxelObj.GetComponent<Voxel>().SetFutureState(1);
                        //if (wew.GetState() == 1)
                        //{
                          // rose.SetFutureState(1);
                        //wuw.SetFutureState(0);
                        //}
                        //else
                        //{
                        //    rose.SetFutureState(1);
                        //}
                       // wew.SetFutureState(1);



                    }
                   
                }


                //age - here is an example of a condition where the cell is "killed" if its age is above a threshhold
                // in this case if this rule is put here after the Game of Life rules just above it, it would override 
                // the game of lie conditions if this condition was true

                //if (currentVoxelObj.GetComponent<Voxel>().GetAge() > 5)
                //{
                //    currentVoxelObj.GetComponent<Voxel>().SetFutureState(0);
                //    ///
                //    /// 
                //    /// 
                //    /// 
                //     wow.SetFutureState(1);
                //     wuw.SetFutureState(0);

                //}


            }

		}
	    
    }

    // Save the CA states - this is run after the future state of all cells is calculated to update/save
    //current state on the current level
	void SaveCA(){

        //counter stores the number of live cells on this level and is incremented below 
        //in the for loop for each cell with a state of 1
        totalAliveCells = 0;
		for(int i =0; i< width; i++){
			for (int j = 0; j < length; j++) {
                GameObject currentVoxelObj = voxelGrid[i, j, 0];
                int currentVoxelState = currentVoxelObj.GetComponent<Voxel>().GetState();
                // Save the voxel state
                GameObject savedVoxel = voxelGrid[i, j, currentFrame];
                savedVoxel.GetComponent<Voxel> ().SetState (currentVoxelState);                
                // Save the voxel age if voxel is alive
                if (currentVoxelState == 1) {
                    int currentVoxelAge = currentVoxelObj.GetComponent<Voxel>().GetAge();
                    savedVoxel.GetComponent<Voxel>().SetAge(currentVoxelAge);
                    totalAliveCells++;
                    //SetCountText();
                    //track oldest voxels
                    if (currentVoxelAge>maxAge)
                    {
                        maxAge = currentVoxelAge;
                    }
                }
			}
		}

        float totalcells = length * width;
        layerdensity = totalAliveCells/ totalcells;

	    if (layerdensity>maxlayerdensity)
	    {
	        maxlayerdensity = layerdensity;
	    }
        //this stores the density of live cells for each entire layer of cells(each level)
        layerDensities[currentFrame] = layerdensity;

    }


    // Separate data based on density (getDensity3dMO)
    void SeparateVoxelsByDensity()
    {
        // Get all the stored desnities from the voxels
        List<int> availableDensities = new List<int>();
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                for (int k = 1; k < height - 1; k++)
                {
                    Voxel currentVoxel = voxelGrid[i, j, k].GetComponent<Voxel>();
                    int currentVoxelDensity = currentVoxel.getDensity3dMO();
                    if (availableDensities.Contains(currentVoxelDensity) == false)
                    {
                        availableDensities.Add(currentVoxelDensity);
                    }
                }
            }
        }
    }




    void SetCountText()
    {
       // popText.text = "Current Population: " + totalAliveCells.ToString();
       
        allPopText.text = "Total Population: " + totalPopulation.ToString();
    }

    void SetCountText2()
    {
        // popText.text = "Current Population: " + totalAliveCells.ToString();
        popText.text = "Current Layer Population: " + totalAliveCells.ToString();
        
    }

    void AllVoxCounter()
    {
        totalPopulation = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    GameObject populant = voxelGrid[i, j, k];
                    int populantState = populant.GetComponent<Voxel>().GetState();
                    if (populantState==1)
                    {
                        totalPopulation++;
                    }
                }
            }
        }
    }


    /// <summary>
    /// SETUP MOORES & VON NEUMANN 3D NEIGHBORS
    /// </summary>
    void SetupNeighbors3d()
    {
        for (int i = 1; i < width-1; i++)
        {
            for (int j = 1; j < length-1; j++)
            {
                for (int k = 1; k < height-1; k++)
                {
                    //the current voxel we are looking at...
                    GameObject currentVoxelObj = voxelGrid[i, j, k];

                    ////SETUP Von Neumann Neighborhood Cells////
                    Voxel[] tempNeighborsVN = new Voxel[6];
                    Voxel[] tempNeighborsYT = new Voxel[8];
                    Voxel[] tempNeighborsNR = new Voxel[3];
                    Voxel[] tempNeighborsMT = new Voxel[2];
                    //left
                    //
                    //START OF MT
                    //Lilac
                    Voxel VoxelWuw = voxelGrid[i + 1, j + 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelLeft(VoxelWuw);
                    tempNeighborsMT[0] = VoxelWuw;
                    //
                    //
                    //Lily2
                    Voxel VoxelLily2 = voxelGrid[i - 1, j - 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelLeft(VoxelLily2);
                    tempNeighborsMT[1] = VoxelLily2;
                    //                   
                    //END OF MT
                    //
                    //
                    //START OF NR
                    //Lilac
                    Voxel VoxelLilac = voxelGrid[i - 1, j, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelLeft(VoxelLilac);
                    tempNeighborsNR[0] = VoxelLilac;
                    //
                    //
                    //Lily
                    Voxel VoxelLily = voxelGrid[i - 1, j - 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelLeft(VoxelLily);
                    tempNeighborsNR[1] = VoxelLily;
                    //
                    //
                    //Tulip
                    Voxel VoxelTulip = voxelGrid[i, j - 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelLeft(VoxelTulip);
                    tempNeighborsNR[2] = VoxelTulip;
                    //
                    //END OF NR
                    //
                    //left
                    Voxel VoxelLeft = voxelGrid[i - 1, j, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelLeft(VoxelLeft);
                    tempNeighborsVN[0] = VoxelLeft;

                    //
                    //MY top left corner
                    Voxel Voxel_1 = voxelGrid[i - 1, j, k + 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(Voxel_1);
                    tempNeighborsYT[0] = Voxel_1;
                    //
                    //
                    // 
                    // 
                    //right
                    Voxel VoxelRight = voxelGrid[i + 1, j, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelRight(VoxelRight);
                    tempNeighborsVN[2] = VoxelRight;

                    ///
                    //MY top right corner
                    Voxel Voxel_2 = voxelGrid[i + 1, j, k + 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(Voxel_2);
                    tempNeighborsYT[1] = Voxel_2;
                    //

                    //back
                    Voxel VoxelBack = voxelGrid[i, j - 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelBack(VoxelBack);
                    tempNeighborsVN[3] = VoxelBack;

                    ///
                    //My bottom left corner

                    Voxel Voxel_3 = voxelGrid[i - 1, j, k - 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(Voxel_3);
                    tempNeighborsYT[2] = Voxel_3;
                    //

                    //front
                    Voxel VoxelFront = voxelGrid[i, j + 1, k].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelFront(VoxelFront);
                    tempNeighborsVN[1] = VoxelFront;

                    ///
                    //My bottom left corner

                    Voxel Voxel_4 = voxelGrid[i + 1, j, k - 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(Voxel_4);
                    tempNeighborsYT[3] = Voxel_4;
                    //

                    //below
                    Voxel VoxelBelow = voxelGrid[i, j, k - 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelBelow(VoxelBelow);
                    tempNeighborsVN[4] = VoxelBelow;

                    //
                    //My Apple Voxel (top back corner)

                    Voxel Voxel_5 = voxelGrid[i, j + 1, k + 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(Voxel_5);
                    tempNeighborsYT[4] = Voxel_5;

                    //above
                    Voxel VoxelAbove = voxelGrid[i, j, k + 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(VoxelAbove);
                    tempNeighborsVN[5] = VoxelAbove;
                    //
                    //My Carrot Voxel (top front corner)

                    Voxel Voxel_6 = voxelGrid[i, j - 1, k + 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(Voxel_6);
                    tempNeighborsYT[5] = Voxel_6;

                    //
                    //My Onion Voxel (top front corner)

                    Voxel Voxel_7 = voxelGrid[i, j - 1, k - 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(Voxel_7);
                    tempNeighborsYT[6] = Voxel_7;

                    //
                    //My Potato Voxel (top front corner)

                    Voxel Voxel_8 = voxelGrid[i, j + 1, k - 1].GetComponent<Voxel>();
                    currentVoxelObj.GetComponent<Voxel>().setVoxelAbove(Voxel_7);
                    tempNeighborsYT[7] = Voxel_8;


                    //Set the Von Neumann Neighbors [] in this Voxel
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dVN(tempNeighborsVN);

                    ////SETUP Moore's Neighborhood////
                    Voxel[] tempNeighborsMO = new Voxel[26];
                    //
                    //
                    //Set the Yekta Neighbors [] in the voxel
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dYT(tempNeighborsYT);
                    //
                    //Set the Matt Neighbors [] in the voxel
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dMT(tempNeighborsMT);
                    //
                    //Set the Noura Neighbors [] in the voxel
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dNR(tempNeighborsNR);

                    int tempcount = 0;
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            for (int p = -1; p < 2; p++)
                            {
                                if ((i + m >= 0) && (i + m < width) && (j + n >= 0) && (j + n < length) && (k + p >= 0) && (k + p < height))
                                {
                                    GameObject neighborVoxelObj = voxelGrid[i + m, j + n, k + p];
                                    if (neighborVoxelObj != currentVoxelObj)
                                    {
                                        Voxel neighborvoxel = voxelGrid[i + m, j + n, k + p].GetComponent<Voxel>();
                                        tempNeighborsMO[tempcount] = neighborvoxel;
                                        tempcount++;
                                    }
                                }
                            }
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setNeighbors3dMO(tempNeighborsMO);
                }
            }
        }
    }
    /// <summary>
    /// Update 3d Densities for Each Voxel
    /// </summary>
    void updateDensities3d()
    {
        for (int i = 1; i < width-1; i++)
        {
            for (int j = 1; j < length-1; j++)
            {
                for (int k = 1; k < currentFrame; k++)
                {
                    GameObject currentVoxelObj = voxelGrid[i, j, k];

                    //UPDATE THE VON NEUMANN NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsVN = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dVN();
                    int alivecount = 0;
                    foreach (Voxel vox in tempNeighborsVN)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setDensity3dVN(alivecount);
                    if (alivecount> maxDensity3dVN) {
                        maxDensity3dVN = alivecount;
                    }

                    //UPDATE THE MOORES NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsMO = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dMO();
                    alivecount = 0;
                    foreach (Voxel vox in tempNeighborsMO)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setDensity3dMO(alivecount);
                    if (alivecount > maxDensity3dMO)
                    {
                        maxDensity3dMO = alivecount;
                    }
                    //UPDATE THE Yekta NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsYT = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dYT();
                    alivecount = 0;

                    foreach (Voxel vox in tempNeighborsYT)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setDensity3dYT(alivecount);
                    if (alivecount > maxDensity3dYT)
                    {
                        maxDensity3dYT = alivecount;
                    }
                    //UPDATE THE Noura NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsNR = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dNR();
                    alivecount = 0;

                    foreach (Voxel vox in tempNeighborsNR)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setDensity3dNR(alivecount);
                    if (alivecount > maxDensity3dNR)
                    {
                        maxDensity3dNR = alivecount;
                    }
                    //UPDATE THE Matt NEIGHBORHOOD DENSITIES FOR EACH VOXEL//
                    Voxel[] tempNeighborsMT = currentVoxelObj.GetComponent<Voxel>().getNeighbors3dMT();
                    alivecount = 0;

                    foreach (Voxel vox in tempNeighborsMT)
                    {
                        if (vox.GetState() == 1)
                        {
                            alivecount++;
                        }
                    }
                    currentVoxelObj.GetComponent<Voxel>().setDensity3dMT(alivecount);
                    if (alivecount > maxDensity3dMT)
                    {
                        maxDensity3dMT = alivecount;
                    }
                }
            }
        }
    }
  
    

    /// <summary>
    /// TESTING VON NEUMANN NEIGHBORS
    /// We can look at the specific voxels above,below,left,right,front,back and color....
    /// We can get all von neumann neighbors and color
    /// </summary>
    /// 
    void VonNeumannLookup()
    {
        //color specific voxel in the grid - [1,1,1]
        GameObject voxel_1 = voxelGrid[1, 1, 1];
        voxel_1.GetComponent<Voxel>().SetState(1);
        voxel_1.GetComponent<Voxel>().VoxelDisplay(1, 0, 0);

        //color specific voxel in the grid - [10,10,10]
        GameObject voxel_2 = voxelGrid[10, 10, 10];
        voxel_2.GetComponent<Voxel>().SetState(1);
        voxel_2.GetComponent<Voxel>().VoxelDisplay(1, 0, 0);

        //get neighbor right and color green
        Voxel voxel_1right = voxel_1.GetComponent<Voxel>().getVoxelRight();
        voxel_1right.SetState(1);
        voxel_1right.VoxelDisplay(0, 1, 0);

        //get neighbor above and color green
        Voxel voxel_1above = voxel_1.GetComponent<Voxel>().getVoxelAbove();
        voxel_1above.SetState(1);
        voxel_1above.VoxelDisplay(1, 0, 1);

        //get neighbor above and color magenta
        Voxel voxel_2above = voxel_2.GetComponent<Voxel>().getVoxelAbove();
        voxel_2above.SetState(1);
        voxel_2above.VoxelDisplay(1, 0, 1);

        //get all VN neighbors of a cell and color yellow
        //color specific voxel in the grid - [12,12,12]
        GameObject voxel_3 = voxelGrid[12, 12, 12];
        Voxel[] tempVNNeighbors = voxel_3.GetComponent<Voxel>().getNeighbors3dVN();
        foreach (Voxel vox in tempVNNeighbors)
        {
            vox.SetState(1);
            vox.VoxelDisplay(1, 1, 0);
        }

    }

    /// <summary>
    /// TESTING MOORES NEIGHBORS
    /// We can look at the specific voxels above,below,left,right,front,back and color....
    /// We can get all von neumann neighbors and color
    /// </summary>
    /// 
    void MooreLookup()
    {
        //get all MO neighbors of a cell and color CYAN
        //color specific voxel in the grid - [14,14,14]
        GameObject voxel_1 = voxelGrid[14, 14, 14];
        Voxel[] tempMONeighbors = voxel_1.GetComponent<Voxel>().getNeighbors3dMO();
        foreach (Voxel vox in tempMONeighbors)
        {
            vox.SetState(1);
            vox.VoxelDisplay(0, 1, 1);
        }

    }
    void PrintAge()
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                for (int k = 1; k < height - 1; k++)
                {
                    Voxel currentVoxel = voxelGrid[i, j, k].GetComponent<Voxel>();
                    print("Voxel[" + i + "][" + j + "][" + k + "] age is: " + currentVoxel.GetAge().ToString());
                }
            }
        }
    }
	void ExportPrepare()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < length; j++)
			{
				for (int k = 0; k < height; k++)
				{
					Voxel currentVoxel = voxelGrid[i, j, k].GetComponent<Voxel>();
					if (currentVoxel.GetState() == 0)
					{
						Destroy(currentVoxel.gameObject);
					}
				}
			}
		}
	}
	//////////////////////////////////////
}


