# -*- Makefile -*-
#
#  This file is part of Siena, a wide-area event notification system.
#  See http://www.cs.colorado.edu/serl/siena/
#
#  Author: See the file AUTHORS for full details. 
#
#  Copyright (C) 2001-2003 University of Colorado
#
#  This program is free software; you can redistribute it and/or
#  modify it under the terms of the GNU General Public License
#  as published by the Free Software Foundation; either version 2
#  of the License, or (at your option) any later version.
#
#  This program is distributed in the hope that it will be useful,
#  but WITHOUT ANY WARRANTY; without even the implied warranty of
#  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#  GNU General Public License for more details.
#
#  You should have received a copy of the GNU General Public License
#  along with this program; if not, write to the Free Software
#  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,
#  USA, or send email to serl@cs.colorado.edu.
#
#
# $Id: Makefile.am,v 1.3 2008-04-01 16:11:13 carzanig Exp $
#
SUBDIRS = .
#
INCLUDES = -I$(top_srcdir)
#
EXTRA_DIST = bootstrap ssbg.spec \
	attr_names.dist bool_values.dist int_values.dist \
	bool_operators.dist double_values.dist double_operators.dist \
	int_operators.dist string_values.dist string_operators.dist \
	types.dist
#
lib_LTLIBRARIES = libssbg.la
libssbg_la_SOURCES = ssbg.cc io_exception.cc

libssbgincludedir = $(includedir)/siena
libssbginclude_HEADERS = siena/ssbg.h

nobase_include_HEADERS = siena/ssbg.h

AM_CFLAGS = -Wall @CFLAGS_prof@
AM_CXXFLAGS = -Wall @CXXFLAGS_prof@
AM_LDFLAGS = @LDFLAGS_prof@

libssbg_la_LDFLAGS= \
 -version-info $(LT_CURRENT):$(LT_REVISION):$(LT_AGE) \
 -release $(LT_RELEASE)

bin_PROGRAMS = ssbgdict ssbg

ssbgdict_SOURCES = gen_dictionary.cc

AM_YFLAGS = -d

ssbg_SOURCES = ssbg_main.cc 
ssbg_LDADD = libssbg.la

.PHONY: release snapshot doc test rpm

release:
	rm -rf .deps */.deps
	$(MAKE) distcheck

snapshot:
	$(MAKE) dist distdir=$(PACKAGE)-`date +%Y-%m-%d`

doc:
	cd doc && $(MAKE) doc

#
# RPM stuff
#
RPMBUILD = @RPMBUILD@
RPMROOT = rpmroot
RPMBASE = rpmbase
RPMTREE = $(RPMBASE) \
	$(RPMBASE)/BUILD $(RPMBASE)/RPMS $(RPMBASE)/SOURCES $(RPMBASE)/SPECS

RPMFLAGS = --define="_topdir `pwd`/$(RPMBASE)" \
	--define='version $(VERSION)' \
	--buildroot="`pwd`/$(RPMROOT)"

$(RPMBASE)/SOURCES/$(PACKAGE)-$(VERSION).tar.gz: dist
	$(CP) $(PACKAGE)-$(VERSION).tar.gz $@

$(RPMBASE)/SPECS/ssbg.spec: ssbg.spec
	$(CP) $< $@

$(RPMTREE):
	mkdir $@

rpm: $(RPMTREE) $(RPMBASE)/SOURCES/$(PACKAGE)-$(VERSION).tar.gz \
		$(RPMBASE)/SPECS/ssbg.spec
	$(RPMBUILD) $(RPMFLAGS) -bb $(RPMBASE)/SPECS/ssbg.spec
	$(MV) $(RPMBASE)/RPMS/*/$(PACKAGE)-$(VERSION)-*.rpm ./
	$(RM) -rf $(RPMBASE)

#
# cleaning up...
#
CLEANFILES = $(RPMBASE) $(RPMROOT)
DISTCLEANFILES = config/stamp-h1 so_locations
