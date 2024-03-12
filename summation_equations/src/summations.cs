using  System.Numerics;
using  System.Collections.Generic;
namespace  summations {


class  summation_functions {
    
    // --------------------------------------------------------------------------
    // Class Variables ----------------------------------------------------------
    private  List<BigInteger[]>  jagged_summation_eq;
    private  List<BigInteger>    summation_divisor;
    
    
    // --------------------------------------------------------------------------
    // Constructor! -------------------------------------------------------------
    public summation_functions()
    {
        jagged_summation_eq  = new List<BigInteger[]>();
        summation_divisor    = new List<BigInteger>() {1,1,2,6} ; // 24
    
        // Base cases for solving summations E[i^x]
        jagged_summation_eq.Add(new BigInteger[]        {1,1}); // E(0 to N)[1]   = (N+1)
        jagged_summation_eq.Add(new BigInteger[]      {0,1,1}); // E(0 to N)[i]   = (1/2)(N)(N+1)
        jagged_summation_eq.Add(new BigInteger[]    {0,1,3,2}); // E(0 to N)[i^2] = (1/6)(N)(N+1)(2N+1)
        // jagged_summation_eq.Add(new BigInteger[] {0,0,6,12,6}); // E(0 to N)[i^3] = (1/24)(N^2)(N+1)^2
    }
    
    
    // --------------------------------------------------------------------------
    // Solve Summation: E(0 to N)[i^x] in terms of "N" for input "x" ------------
    public void solve_summ_eq(int i_exp)
    {
	if(i_exp < jagged_summation_eq.Count)
	{
	    // Already solved...
            return;
	}
	else if(i_exp > jagged_summation_eq.Count)
	{
	    // Solve previous if necessary.
            solve_summ_eq(i_exp-1);
	}

        // if(i_exp % 2){ solve_summ_eq_odd_recurse(i_exp); } // Faster
        // else{          solve_summ_eq_recurse(i_exp);     } // Default
	solve_summ_eq_recurse(i_exp);
	return;
    }
    
    
    // --------------------------------------------------------------------------
    // Calculate Summation  E(0 to N)[i^x] in terms of "N" for input "x" --------
    public BigInteger calc_summation(int i_exp, int i_N)
    {
	BigInteger bi_val = 0;

	// Check if equation is ready.
	if (i_exp >= jagged_summation_eq.Count)
	{
    	    return bi_val;
	}

        // Solves polynomial equation: ((A) + (B)N + (C)N^2 + (D)N^3 + ...) -----
        for(int ii = (i_exp+1); ii >= 0; ii--)
	{
            bi_val *= i_N;
            bi_val += jagged_summation_eq[i_exp][ii];
	}

	return bi_val;
    }

    
    // --------------------------------------------------------------------------
    // <>
    public string get_str_summation(int i_exp)
    {
	if(i_exp >= jagged_summation_eq.Count)
	{
	    return "NULL";
	}

        string t_str = "E(0 to N)[i^"+i_exp.ToString()+"] := (1/"+summation_divisor[i_exp+1].ToString()+")( "+jagged_summation_eq[i_exp][0].ToString()+" + "+jagged_summation_eq[i_exp][1].ToString()+"N";

        for(int ii=2; ii<=(i_exp+1); ii++)
	{
            t_str += " + "+jagged_summation_eq[i_exp][ii].ToString()+"(N^"+ii.ToString()+")" ;
	}
        t_str += " )";
	return t_str;
    } 

    // --------------------------------------------------------------------------
    // Default function to solve a summation based on previous summations. ------
    private void solve_summ_eq_recurse(int i_exp)
    {
        // ----------------------------------------------------------------------
        //  Using Pascal's triangle, can solve any E[i^x] using:  E[i^(x+1)] = E[(i+1)^(x+1)] - (N+1)^(x+1)
        //      - EG: If solving for E[i^3], then input i_exp == 3; need Pascal 4th row: 1,4,6,4,1
        //      - Recursive, IE: for E[i^3], need solutions to E[1], E[i], and E[i^2]
        // ----------------------------------------------------------------------
	
	// Pascals triangle!
        BigInteger   pascals_val      = 1;
        BigInteger[] pascals_triangle = new BigInteger[i_exp + 2];
        
        pascals_triangle[0]         = 1;
        pascals_triangle[i_exp + 1] = 1;
        
        for(int ii = 1; ii <= (i_exp+1)/2; ii++)
        {
            pascals_val = (pascals_val * (2+i_exp-ii)) / (ii);
            pascals_triangle[ii]         = pascals_val;
            pascals_triangle[i_exp+1-ii] = pascals_val;
        }

        // ----------------------------------------------------------------------
	//  Solve equation { E[i^(x+1)] = E[(i+1)^(x+1)] - (N+1)^(x+1) } for E[i^x]
        // ----------------------------------------------------------------------

	BigInteger[] next_summ = new BigInteger[i_exp + 2];
        
        // Initialize to Pascals!
        for(int ii = 0; ii < (i_exp+2); ii++)
        {
            next_summ[ii] = pascals_triangle[ii] * summation_divisor[i_exp];
        }
        
        // Subtract weighted summations of other E[i^(y)].
        for(int ii = 0; ii < i_exp; ii++)
        {
            for(int jj = 0; jj < (ii+2); jj++)
            {
                next_summ[jj] -=
	            pascals_triangle[ii] * (summation_divisor[i_exp] / summation_divisor[ii+1]) * jagged_summation_eq[ii][jj];
            }
        }

        // Add new elements and return.
        jagged_summation_eq.Add(next_summ);
        summation_divisor.Add(summation_divisor[i_exp]*(i_exp+1));
	return;
    }
    
    // --------------------------------------------------------------------------
    // Solves summations of odd exponents, recursively uses other odd summations.
    private void solve_summ_eq_odd(int i_exp)
    {
        // ----------------------------------------------------------------------
     	// Using  {E(1 to N)[i]}^y - {E(1 to N-1)[i]}^y :
     	//    - {E(1 to N)[i]}^2 - {E(1 to N-1)[i]}^2  ==  (1/2 ) (N^2) ( 2N^1 )              
     	//    - {E(1 to N)[i]}^3 - {E(1 to N-1)[i]}^3  ==  (1/4 ) (N^3) ( 3N^2 + 0N^1 +  1N^0 )          
     	//    - {E(1 to N)[i]}^4 - {E(1 to N-1)[i]}^4  ==  (1/8 ) (N^4) ( 4N^3 + 0N^2 +  4N^1 + 0N^0 )        
     	//    - {E(1 to N)[i]}^5 - {E(1 to N-1)[i]}^5  ==  (1/16) (N^5) ( 5N^4 + 0N^3 + 10N^2 + 0N^1 + 1N^0 )      
     	//    - {E(1 to N)[i]}^6 - {E(1 to N-1)[i]}^6  ==  (1/32) (N^6) ( 6N^5 + 0N^4 + 20N^3 + 0N^2 + 6N^1 + 0N^0 )
        // ----------------------------------------------------------------------
        // To solve for E[i^x], use {E(1 to N)[i]}^y - {E(1 to N-1)[i]}^y where:
        //    -   y = (x+1)/2
        //    -   x = 2y-1
        // ----------------------------------------------------------------------
     	// EG:
	//  -   {E(1 to N)[i]}^6 - {E(1 to N-1)[i]}^6  == (1/32)(N^6)( 6N^5 + 0N^4 + 20N^3 + 0N^2 + 6N^1 + 0N^0 )
	//  -   (2^5) {E(1 to N)[i]}^6                 == 6E[i^11] + 20E[i^9] + 6E[i^7]
	//  -   6E[i^11]                               ==  (2^5)((N)(N-1)/2)^6 - 20E[i^9] - 6E[i^7]
        // ----------------------------------------------------------------------
        
	return;
    }
    
} // class  summation_functions
} // namespace  summations
