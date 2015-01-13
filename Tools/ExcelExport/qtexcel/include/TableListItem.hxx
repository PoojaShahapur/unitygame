#ifndef TABLELISTITEM_H
#define TABLELISTITEM_H

#include <QWidget>

namespace Ui
{
	class TableListItem;
}

class TableListItem : public QWidget
{
	Q_OBJECT
private:
	Ui::TableListItem *m_ui;

public:
	explicit TableListItem(QWidget *parent);

signals:
	public slots :
};

#endif // TABLELISTITEM_H