using System;
using summations;

namespace  test_simple {
class  Program {

    const int MAX_EXP = 6;

    static void Main(string[] args)
    {
	Console.WriteLine("\n"+"---------------------------------------------------------------------");
        summation_functions sf_example = new summation_functions();
	
	Console.WriteLine("\nSummation Equations:");
	
	for(int ii=1; ii <= MAX_EXP; ii++){
	    sf_example.solve_summ_eq(ii);
	    Console.WriteLine(sf_example.get_str_summation(ii));
	}
	
	Console.WriteLine("\n"+"---------------------------------------------------------------------");
    }

} // class  Program
} // namespace  test_simple
