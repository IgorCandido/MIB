// -*- C++ -*-
//
//  This file is part of Siena, a wide-area event notification system.
//  See http://www.cs.colorado.edu/serl/siena/
//
//  Authors:See the file AUTHORS for full details. 
//
//  Copyright (C) 2002-2003 University of Colorado
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,
//  USA, or send email to serl@cs.colorado.edu.
//
// $Id: ssbg.h,v 1.2 2004/12/13 15:43:15 carzanig Exp $
//
#ifndef _SSBG_H
#define _SSBG_H

#include <exception>
#include <map>
#include <vector>
#include <list>
#include <string>
#include <iostream>

using namespace std;

namespace ssbg
{
  enum TypeId { BOOL = 'b', DOUBLE = 'd', INT = 'i', STRING = 's' };

  class attribute
  {
  public:
    string name;
    TypeId type;
    union
    {
      int int_val;
      double double_val;
      bool bool_val;
    };
    string string_val;

    attribute();
    attribute( const string& name, bool val );
    attribute( const string& name, double val );
    attribute( const string& name, int val );
    attribute( const string& name, const string& val );
    attribute( const attribute& a );
  };

  class constraint
  {
  public:
    string name;
    TypeId type;
    string op;
    union
    {
      int int_val;
      double double_val;
      bool bool_val;
    };
    string string_val;

    constraint();
    constraint( const string& name, const string& op, bool val );
    constraint( const string& name, const string& op, double val );
    constraint( const string& name, const string& op, int val );
    constraint( const string& name, const string& op, const string& val );
    constraint( const constraint& c );
  };

  typedef list<attribute> notification;

  typedef list<constraint> filter;
  typedef list<filter> predicate;

  typedef unsigned int weight_t;
  
  typedef list<string> filter_container;
  typedef vector<filter_container> if_container;
  
  /** "weighted" set
   *
   *  represents a set of elements, each having a "weight" parameter
   **/
  template <class T>
  class wset {
  private:
    map<weight_t, T>	wmap;
    weight_t		weight_size;
    static weight_t rand_normal(weight_t max) {
      return (weight_t) (1.0*max*rand()/(RAND_MAX+1.0));
    }
      
  public:
    void clear() {
      wmap.clear();
      weight_size = 0;
    }
      
    void add(weight_t w, const T & v) {
      if (w > 0) {
	weight_size += w;
	wmap.insert(wmap.end(), 
		    typename map<weight_t, T>::value_type(weight_size - 1, v));
      }
    }
      
    T rand_pick() const {
      return (*wmap.lower_bound(rand_normal(weight_size))).second;
    }
      
    bool read_from(istream & in) {
      clear();
      T v;
      weight_t w;
      while (in >> w >> v) 
	add(w, v);
      return true;
    }
  };

  /**
   * @brief main class implementing the functionality of the SSBG
   * library.  This has methods for loading the distribution files
   * and for getting new filters and notifications.
   */
  class SSBG
    {
      /**
       * @brief the lower bound on the range of number of attributes
       * per message
       */
      int m_attr_min;

      /**
       * @brief the upper bound on the range of number of attributes
       * per message.
       */
      int m_attr_max;

      /**
       * @brief the lower bound on the range of number of constraints
       * per filter.
       */
      int m_constr_min;

      /**
       * @brief the upper bound on the range of number of constraints
       * per filter.
       */
      int m_constr_max;

      /**
       * @brief the lower bound on the range of number of filters
       * per predicate.
       */
      int m_filters_min;

      /**
       * @brief the upper bound on the range of number of filters
       * per predicate.
       */
      int m_filters_max;

      /**
       * @brief flag that determines if the first type associated with
       * a particular name is reused whenever the name is picked in the
       * future.
       */
      bool m_reuse_types;

      map<string,char> m_type_map;

      wset<string> m_attr_dist;
      wset<string> m_constr_dist;
      wset<string> m_op_bool_dist;
      wset<string> m_op_double_dist;
      wset<string> m_op_int_dist;
      wset<string> m_op_string_dist;
      wset<char> m_types_dist;
      wset<bool> m_bool_dist;
      wset<double> m_double_dist;
      wset<int> m_int_dist;
      wset<string> m_str_dist;

      template<class T> void load_distribution( const char* fname, wset<T>& dist );

      inline char pick_type( const string& str );

    public:
      /**
       * @brief default constructor initializes all of the private
       * data members.
       */
      SSBG( int attr_min, int attr_max, int constr_min, int constr_max,
	    int filters_min, int filters_max, bool reuse_types = false );

      /**
       * @brief returns the value of the attr_min attribute.
       */
      int attr_min() const;

      /**
       * @brief returns the value of the attr_max attribute.
       */
      int attr_max() const;

      /**
       * @brief returns the value of the constr_min attribute.
       */
      int constr_min() const;

      /**
       * @brief returns the value of the constr_max attribute.
       */
      int constr_max() const;

      /**
       * @brief returns the value of the filters_min attribute.
       */
      int filters_min() const;

      /**
       * @brief returns the value of the filters_max attribute.
       */
      int filters_max() const;

      /**
       * @brief loads the specified distribution files into memory.
       * Throws <code>io_exception</code> on errors.
       */
      void load_distributions( const char* bool_dist_f,
			       const char* double_dist_f,
			       const char* int_dist_f,
			       const char* str_dist_f,
			       const char* types_dist_f,
			       const char* op_bool_dist_f,
			       const char* op_double_dist_f,
			       const char* op_int_dist_f,
			       const char* op_string_dist_f,
			       const char* attr_dist_f,
			       const char* constr_dist_f );

      /**
       * @brief allocates a new filter object and fills it in
       * according to the internal distributions.
       */
      predicate* new_predicate();

      /**
       * @brief clears the filter that is passed in and fills it
       * in with new values according to the internal distributions.
       */
      predicate& new_predicate( predicate& p );

      /**
       * @brief allocates a new notification objects and fill is in
       * according to the internal distributions.
       */
      notification* new_notification();

      /**
       * @brief clears the notification that is passed in and fills it
       * in with new values according to the internal distributions.
       */
      notification& new_notification( notification& f );
    };

  class io_exception : public exception
  {
    string m_arg;
    
  public:
    io_exception( const string& arg ) throw ();
    
    virtual ~io_exception() throw();
    
    virtual const char* what() const throw ();
  };

  /** @brief random value with uniform distribution in [0,max)
   **/
  int rand_normal(int max);

  /** @brief random value with uniform distribution in [min,max)
   **/
  int rand_normal_range(int min, int max);

  unsigned long rand_poisson_delta(unsigned long mean);
};

#endif // _SSBG_H
