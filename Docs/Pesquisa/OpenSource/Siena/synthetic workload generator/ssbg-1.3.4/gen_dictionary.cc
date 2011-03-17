//
// $Id: gen_dictionary.cc,v 1.1 2004/01/26 18:01:58 carzanig Exp $
//
#include "ssbconf.h"

#include <sys/time.h>

#include <cstdlib>
#include <iostream>
#include <iterator>
#include <fstream>
#include <algorithm>
#include <vector>
#include <string>

using namespace std;

void print_usage(const char *name) 
{
    cerr << "usage: " << name << " [options...]\n\
options:\n\
\t-num <num>\t number of words\n\
\t-dict <file>\t dictionary filename\n\
\t-seed <num>\t random seed\n\
\t-dist n|l|z\t add prevalence distribution (n=normal, l=linear, z=Zipf)\n\
" << endl;
    exit(1);
}

int par_num;
long par_seed;
string par_dict = "/usr/share/dict/words";
char par_dist = '\0';

vector<string> attr_dict;
vector<string> str_dict;

inline int rand_normal(int max) {
    return (int) (1.0*max*rand()/(RAND_MAX+1.0));
}

inline int rand_normal_range(int min, int max) {
    return min + rand_normal(max - min);
}

int main(int argc, char *argv[]) 
{
    struct timeval tv;
    gettimeofday(&tv, NULL);
    par_seed = tv.tv_usec;

    for (int i=1; i< argc; ++i) {	// parses command-line parameters
	if (strcmp(argv[i], "-num")==0) {
	    if (++i < argc) {
		par_num = atoi(argv[i]);
	    } else {
		print_usage(argv[0]);
	    }
	} else if (strcmp(argv[i], "-seed")==0) {
	    if (++i < argc) {
		par_seed = atol(argv[i]);
	    } else {
		print_usage(argv[0]);
	    }
	} else if (strcmp(argv[i], "-dict")==0) {
	    if (++i < argc) {
		par_dict = argv[i];
	    } else {
		print_usage(argv[0]);
	    }
	} else if (strcmp(argv[i], "-dist")==0) {
	    if (++i < argc) {
		par_dist = *argv[i];
	    } else {
		print_usage(argv[0]);
	    }
	} else {
	    print_usage(argv[0]);
	}
    }

    srand(par_seed);

    ifstream dict_stream(par_dict.c_str());
    if (!dict_stream) {
	cerr << "error: could not open dictionary file: " << par_dict << endl;
	return 1;
    }

    vector<string> attr_dict;
    back_insert_iterator<vector<string> > bi(attr_dict);
    copy(istream_iterator<string>(dict_stream), istream_iterator<string>(), 
	 bi);

    random_shuffle(attr_dict.begin(), attr_dict.end(), rand_normal);

    attr_dict.resize(par_num);

    vector<string>::const_iterator vi;
    switch(par_dist) {
    case 'n': 
	for(vi = attr_dict.begin(); vi != attr_dict.end(); ++vi) 
	    cout << "1 " << *vi << endl;
	break;

    case 'l': {
	unsigned int dist_weight = 1;
	for(vi = attr_dict.begin(); vi != attr_dict.end(); ++vi) 
	    cout << dist_weight++ << ' ' << *vi << endl; 
	break;
    }
    case 'z': {
	unsigned int count = 1;
	for(vi = attr_dict.begin(); vi != attr_dict.end(); ++vi) 
	    cout << (par_num / count++) << ' ' << *vi << endl;
	break;
    }
    default:
	for(vi = attr_dict.begin(); vi != attr_dict.end(); ++vi) 
	    cout << *vi << endl;
    }
}
